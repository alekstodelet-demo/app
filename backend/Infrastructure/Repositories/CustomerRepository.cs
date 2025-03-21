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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;

        public CustomerRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<Customer>>> GetAll()
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""pin"" AS ""Pin"",
                        ""okpo"" AS ""Okpo"",
                        ""postal_code"" AS ""PostalCode"",
                        ""ugns"" AS ""Ugns"",
                        ""reg_number"" AS ""RegNumber"",
                        ""organization_type_id"" AS ""OrganizationTypeId"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""name"" AS ""Name"",
                        ""address"" AS ""Address"",
                        ""director"" AS ""Director"",
                        ""nomer"" AS ""Nomer""
                        
                    FROM ""customer"";
                ";

                var models = await _dbConnection.QueryAsync<Customer>(sql, transaction: _dbTransaction);
                
                var results = models.Select(model => FromCustomerModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get all customer", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<Customer>> GetOneByID(int id)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""pin"" AS ""Pin"",
                        ""okpo"" AS ""Okpo"",
                        ""postal_code"" AS ""PostalCode"",
                        ""ugns"" AS ""Ugns"",
                        ""reg_number"" AS ""RegNumber"",
                        ""organization_type_id"" AS ""OrganizationTypeId"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""name"" AS ""Name"",
                        ""address"" AS ""Address"",
                        ""director"" AS ""Director"",
                        ""nomer"" AS ""Nomer""
                        
                    FROM ""customer"" WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"Customer with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                var result = FromCustomerModel(model);

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
                var model = ToCustomerModel(domain);

                var sql = @"
                    INSERT INTO ""customer""(""pin"", ""okpo"", ""postal_code"", ""ugns"", ""reg_number"", ""organization_type_id"", ""created_at"", ""updated_at"", ""created_by"", ""updated_by"", ""name"", ""address"", ""director"", ""nomer"" ) 
                    VALUES (@Pin, @Okpo, @PostalCode, @Ugns, @RegNumber, @OrganizationTypeId, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @Name, @Address, @Director, @Nomer) RETURNING id
                ";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add Customer", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(Customer domain)
        {
            try
            {
                var model = ToCustomerModel(domain);

                var sql = @"
                    UPDATE ""customer"" SET 
                    ""id"" = @Id, ""pin"" = @Pin, ""okpo"" = @Okpo, ""postal_code"" = @PostalCode, ""ugns"" = @Ugns, ""reg_number"" = @RegNumber, ""organization_type_id"" = @OrganizationTypeId, ""updated_at"" = @UpdatedAt, ""updated_by"" = @UpdatedBy, ""name"" = @Name, ""address"" = @Address, ""director"" = @Director, ""nomer"" = @Nomer 
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
                return Result.Fail(new ExceptionalError("Failed to update Customer", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<Customer>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""pin"" AS ""Pin"",
                        ""okpo"" AS ""Okpo"",
                        ""postal_code"" AS ""PostalCode"",
                        ""ugns"" AS ""Ugns"",
                        ""reg_number"" AS ""RegNumber"",
                        ""organization_type_id"" AS ""OrganizationTypeId"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""name"" AS ""Name"",
                        ""address"" AS ""Address"",
                        ""director"" AS ""Director"",
                        ""nomer"" AS ""Nomer""
                        
                    FROM ""customer""
                    OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<Customer>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM ""customer""";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => FromCustomerModel(model)).ToList();

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
                var sql = @"DELETE FROM ""customer"" WHERE id = @Id";
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

        
        public async Task<List<Customer>> GetByOrganizationTypeId(int OrganizationTypeId)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""pin"" AS ""Pin"",
                        ""okpo"" AS ""Okpo"",
                        ""postal_code"" AS ""PostalCode"",
                        ""ugns"" AS ""Ugns"",
                        ""reg_number"" AS ""RegNumber"",
                        ""organization_type_id"" AS ""OrganizationTypeId"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""name"" AS ""Name"",
                        ""address"" AS ""Address"",
                        ""director"" AS ""Director"",
                        ""nomer"" AS ""Nomer""
                        
                    FROM ""customer"" WHERE ""organization_type_id"" = @OrganizationTypeId";
                var models = await _dbConnection.QueryAsync<Customer>(sql, new { OrganizationTypeId }, transaction: _dbTransaction);
                return models.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get customer by id", ex);
            }
        }
        

        private CustomerModel ToCustomerModel(Customer model)
        {
            return new CustomerModel
            {
                Id = model.Id,
                Pin = model.Pin,
                Okpo = model.Okpo,
                PostalCode = model.PostalCode,
                Ugns = model.Ugns,
                RegNumber = model.RegNumber,
                OrganizationTypeId = model.OrganizationTypeId,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                Name = model.Name,
                Address = model.Address,
                Director = model.Director,
                Nomer = model.Nomer
                
            };
        }

        private Customer FromCustomerModel(Customer model)
        {
            return new Customer
            {
                Id = model.Id,
                Pin = model.Pin,
                Okpo = model.Okpo,
                PostalCode = model.PostalCode,
                Ugns = model.Ugns,
                RegNumber = model.RegNumber,
                OrganizationTypeId = model.OrganizationTypeId,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                Name = model.Name,
                Address = model.Address,
                Director = model.Director,
                Nomer = model.Nomer
                
            };
        }
    }
}
