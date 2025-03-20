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
                                   value AS ""Value"",
                                   type_id AS ""TypeId"",
                                   allow_notification AS ""AllowNotification"",
                                   customer_id AS ""CustomerId"",
                                   created_at AS ""CreatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_at AS ""UpdatedAt"",
                                   updated_by AS ""UpdatedBy""
                            FROM customer_contact;";
                var models = await _dbConnection.QueryAsync<CustomerContact>(sql, transaction: _dbTransaction);

                return Result.Ok(models.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get CustomerContact", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<List<CustomerContact>>> GetByCustomerId(int customerId)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   value AS ""Value"",
                                   type_id AS ""TypeId"",
                                   allow_notification AS ""AllowNotification"",
                                   customer_id AS ""CustomerId"",
                                   created_at AS ""CreatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_at AS ""UpdatedAt"",
                                   updated_by AS ""UpdatedBy""
                            FROM customer_contact WHERE customer_id = @CustomerId;";
                var models = await _dbConnection.QueryAsync<CustomerContact>(sql, new { CustomerId = customerId }, transaction: _dbTransaction);

                return Result.Ok(models.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get CustomerContact", ex)
                    .WithMetadata("ErrorCode", "FETCH_BY_CUSTOMER_FAILED"));
            }
        }

        public async Task<Result<CustomerContact>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   value AS ""Value"",
                                   type_id AS ""TypeId"",
                                   allow_notification AS ""AllowNotification"",
                                   customer_id AS ""CustomerId"",
                                   created_at AS ""CreatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_at AS ""UpdatedAt"",
                                   updated_by AS ""UpdatedBy""
                            FROM customer_contact WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<CustomerContact>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"CustomerContact with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok(model);
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
                var sql = @"INSERT INTO customer_contact(value, type_id, allow_notification, customer_id, created_at, created_by, updated_at, updated_by) 
                    VALUES (@Value, @TypeId, @AllowNotification, @CustomerId, NOW(), 1, NOW(), 1) RETURNING id";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, domain, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add CustomerContact", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(CustomerContact domain)
        {
            try
            {
                var sql = @"UPDATE customer_contact 
                            SET value = @Value,
                                type_id = @TypeId,
                                allow_notification = @AllowNotification,
                                customer_id = @CustomerId,
                                updated_at = NOW(),
                                updated_by = 1
                            WHERE id = @Id";

                var affected = await _dbConnection.ExecuteAsync(sql, domain, transaction: _dbTransaction);
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

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = "DELETE FROM customer_contact WHERE id = @Id";
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
        public async Task<Result<PaginatedList<CustomerContact>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   value AS ""Value"",
                                   type_id AS ""TypeId"",
                                   allow_notification AS ""AllowNotification"",
                                   customer_id AS ""CustomerId"",
                                   created_at AS ""CreatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_at AS ""UpdatedAt"",
                                   updated_by AS ""UpdatedBy""
                            FROM customer_contact
                            OFFSET @pageSize * (@pageNumber - 1) LIMIT @pageSize;";
                var models = await _dbConnection.QueryAsync<CustomerContact>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT COUNT(*) FROM customer_contact";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                return Result.Ok(new PaginatedList<CustomerContact>(models.ToList(), totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get paginated CustomerContact", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }
    }
}
