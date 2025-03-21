using System.Data;
using Dapper;
using Domain.Entities;
using Application.Repositories;
using Infrastructure.Data.Models;
using Application.Exceptions;
using Application.Models;
using System;
using FluentResults;

namespace Infrastructure.Repositories
{
    public class CustomerRequisiteRepository : ICustomerRequisiteRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;

        public CustomerRequisiteRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<CustomerRequisite>>> GetAll()
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""payment_account"" AS ""PaymentAccount"",
                        ""bank"" AS ""Bank"",
                        ""bik"" AS ""Bik"",
                        ""organization_id"" AS ""OrganizationId"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy""
                        
                    FROM ""customer_requisite"";
                ";

                var models = await _dbConnection.QueryAsync<CustomerRequisite>(sql, transaction: _dbTransaction);
                
                var results = models.Select(model => FromCustomerRequisiteModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get all customer_requisite", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<CustomerRequisite>> GetOneByID(int id)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""payment_account"" AS ""PaymentAccount"",
                        ""bank"" AS ""Bank"",
                        ""bik"" AS ""Bik"",
                        ""organization_id"" AS ""OrganizationId"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy""
                        
                    FROM ""customer_requisite"" WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<CustomerRequisite>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"CustomerRequisite with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                var result = FromCustomerRequisiteModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get CustomerRequisite", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(CustomerRequisite domain)
        {
            try
            {
                var model = ToCustomerRequisiteModel(domain);

                var sql = @"
                    INSERT INTO ""customer_requisite""(""payment_account"", ""bank"", ""bik"", ""organization_id"", ""created_at"", ""updated_at"", ""created_by"", ""updated_by"" ) 
                    VALUES (@PaymentAccount, @Bank, @Bik, @OrganizationId, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy) RETURNING id
                ";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add CustomerRequisite", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(CustomerRequisite domain)
        {
            try
            {
                var model = ToCustomerRequisiteModel(domain);

                var sql = @"
                    UPDATE ""customer_requisite"" SET 
                    ""id"" = @Id, ""payment_account"" = @PaymentAccount, ""bank"" = @Bank, ""bik"" = @Bik, ""organization_id"" = @OrganizationId, ""updated_at"" = @UpdatedAt, ""updated_by"" = @UpdatedBy 
                    WHERE id = @Id";
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
                return Result.Fail(new ExceptionalError("Failed to update CustomerRequisite", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<CustomerRequisite>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""payment_account"" AS ""PaymentAccount"",
                        ""bank"" AS ""Bank"",
                        ""bik"" AS ""Bik"",
                        ""organization_id"" AS ""OrganizationId"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy""
                        
                    FROM ""customer_requisite""
                    OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<CustomerRequisite>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM ""customer_requisite""";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => FromCustomerRequisiteModel(model)).ToList();

                var domainItems = results;

                return Result.Ok(new PaginatedList<CustomerRequisite>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get CustomerRequisite", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM ""customer_requisite"" WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("CustomerRequisite not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete CustomerRequisite", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        
        public async Task<List<CustomerRequisite>> GetByOrganizationId(int OrganizationId)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""payment_account"" AS ""PaymentAccount"",
                        ""bank"" AS ""Bank"",
                        ""bik"" AS ""Bik"",
                        ""organization_id"" AS ""OrganizationId"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy""
                        
                    FROM ""customer_requisite"" WHERE ""organization_id"" = @OrganizationId";
                var models = await _dbConnection.QueryAsync<CustomerRequisite>(sql, new { OrganizationId }, transaction: _dbTransaction);
                return models.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get customer_requisite by id", ex);
            }
        }
        

        private CustomerRequisiteModel ToCustomerRequisiteModel(CustomerRequisite model)
        {
            return new CustomerRequisiteModel
            {
                Id = model.Id,
                PaymentAccount = model.PaymentAccount,
                Bank = model.Bank,
                Bik = model.Bik,
                OrganizationId = model.OrganizationId,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy
                
            };
        }

        private CustomerRequisite FromCustomerRequisiteModel(CustomerRequisite model)
        {
            return new CustomerRequisite
            {
                Id = model.Id,
                PaymentAccount = model.PaymentAccount,
                Bank = model.Bank,
                Bik = model.Bik,
                OrganizationId = model.OrganizationId,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy
                
            };
        }
    }
}
