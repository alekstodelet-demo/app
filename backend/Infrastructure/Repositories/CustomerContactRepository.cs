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
    public class CustomerContactRepository : ICustomerContactRepository
    {
        private IDbTransaction? _dbTransaction;
        private IDbConnection _dbConnection;
        private EncryptionService _crypt;

        public CustomerContactRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _crypt = new EncryptionService(configuration);
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<CustomerContact>>> GetAll()
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   value AS ""value""
                            FROM CustomerContact;";
                var models = await _dbConnection.QueryAsync<CustomerContact>(sql, transaction: _dbTransaction);

                var results = models.Select(model => FromCustomerContactModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get CustomerContact", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<CustomerContact>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   value AS ""value"",
                            FROM CustomerContact WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<CustomerContact>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"CustomerContact with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                var result = FromCustomerContactModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get CustomerContact", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(CustomerContact domain)
        {
            try
            {
                var model = ToCustomerContactModel(domain);
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = 1;
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;

                var sql = @"INSERT INTO customer_contact(value, created_at, updated_at, created_by, updated_by) 
                    VALUES (@value,
                            @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy) RETURNING id";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add method error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Result.Fail(new ExceptionalError("Failed to add CustomerContact", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(CustomerContact domain)
        {
            try
            {
                var model = ToCustomerContactModel(domain);
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;

                var sql = "UPDATE customer_contact SET Value = @Value" +
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
                return Result.Fail(new ExceptionalError("Failed to update CustomerContact", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<CustomerContact>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   value as ""value""
                            FROM customer_contact
                            OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<CustomerContact>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM customer_contact";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => FromCustomerContactModel(model)).ToList();

                var domainItems = results;

                return Result.Ok(new PaginatedList<CustomerContact>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get CustomerContact", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM customer_contact WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("CustomerContact not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete CustomerContact", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        private CustomerContactModel ToCustomerContactModel(CustomerContact model)
        {
            return new CustomerContactModel
            {
                Id = model.Id,
                Value = model.Value
            };
        }

        private CustomerContact FromCustomerContactModel(CustomerContact model)
        {
            return new CustomerContact
            {
                Id = model.Id,
                Value = model.Value
            };
        }
    }
}