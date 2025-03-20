using System.Data;
using System.Security.Cryptography;
using Dapper;
using Domain.Entities;
using Application.Repositories;
using Application.Models;
using FluentResults;
using Infrastructure.Data.Models;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private IDbTransaction? _dbTransaction;
        private IDbConnection _dbConnection;
        private EncryptionService _crypt;

        public CustomerRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _crypt = new EncryptionService(configuration);
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<Customer>>> GetAll()
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   pin AS ""Pin""
                            FROM Customer;";
                var models = await _dbConnection.QueryAsync<Customer>(sql, transaction: _dbTransaction);

                var results = models.Select(model => DecryptCustomerModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Customer", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<Customer>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   pin AS ""Pin"",
                            FROM Customer WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"Customer with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                var result = DecryptCustomerModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Customer", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(Customer domain)
        {
            try
            {
                var model = EncryptCustomerModel(domain);
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = 1;
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;

                var sql = @"INSERT INTO Customer(pin, created_at, updated_at, created_by, updated_by) 
                    VALUES (@Pin,
                            @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy) RETURNING id";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add method error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Result.Fail(new ExceptionalError("Failed to add Customer", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(Customer domain)
        {
            try
            {
                var model = EncryptCustomerModel(domain);
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;

                var sql = "UPDATE Customer SET pin = @Pin" +
                          "updated_at = @UpdatedAt, updated_by = @UpdatedBy WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, model, transaction: _dbTransaction);
                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("Not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to update Customer", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<Customer>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   pin as ""Pin""
                            FROM Customer
                            OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<Customer>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM Customer";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => DecryptCustomerModel(model)).ToList();

                var domainItems = results;

                return Result.Ok(new PaginatedList<Customer>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Customer", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM Customer WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("Customer not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete Customer", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        private CustomerModel EncryptCustomerModel(Customer model)
        {
            return new CustomerModel
            {
                Id = model.Id,
                Pin = model.Pin
            };
        }

        private Customer DecryptCustomerModel(Customer model)
        {
            return new Customer
            {
                Id = model.Id,
                Pin = model.Pin
            };
        }
    }
}