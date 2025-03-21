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
    public class RepresentativeRepository : IRepresentativeRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;

        public RepresentativeRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<Representative>>> GetAll()
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""first_name"" AS ""FirstName"",
                        ""second_name"" AS ""SecondName"",
                        ""pin"" AS ""Pin"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""company_id"" AS ""CompanyId"",
                        ""has_access"" AS ""HasAccess"",
                        ""type_id"" AS ""TypeId"",
                        ""last_name"" AS ""LastName""
                        
                    FROM ""representative"";
                ";

                var models = await _dbConnection.QueryAsync<Representative>(sql, transaction: _dbTransaction);
                
                var results = models.Select(model => FromRepresentativeModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get all representative", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<Representative>> GetOneByID(int id)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""first_name"" AS ""FirstName"",
                        ""second_name"" AS ""SecondName"",
                        ""pin"" AS ""Pin"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""company_id"" AS ""CompanyId"",
                        ""has_access"" AS ""HasAccess"",
                        ""type_id"" AS ""TypeId"",
                        ""last_name"" AS ""LastName""
                        
                    FROM ""representative"" WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<Representative>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"Representative with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                var result = FromRepresentativeModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Representative", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(Representative domain)
        {
            try
            {
                var model = ToRepresentativeModel(domain);

                var sql = @"
                    INSERT INTO ""representative""(""first_name"", ""second_name"", ""pin"", ""created_at"", ""updated_at"", ""created_by"", ""updated_by"", ""company_id"", ""has_access"", ""type_id"", ""last_name"" ) 
                    VALUES (@FirstName, @SecondName, @Pin, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @CompanyId, @HasAccess, @TypeId, @LastName) RETURNING id
                ";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add Representative", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(Representative domain)
        {
            try
            {
                var model = ToRepresentativeModel(domain);

                var sql = @"
                    UPDATE ""representative"" SET 
                    ""id"" = @Id, ""first_name"" = @FirstName, ""second_name"" = @SecondName, ""pin"" = @Pin, ""updated_at"" = @UpdatedAt, ""updated_by"" = @UpdatedBy, ""company_id"" = @CompanyId, ""has_access"" = @HasAccess, ""type_id"" = @TypeId, ""last_name"" = @LastName 
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
                return Result.Fail(new ExceptionalError("Failed to update Representative", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<Representative>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""first_name"" AS ""FirstName"",
                        ""second_name"" AS ""SecondName"",
                        ""pin"" AS ""Pin"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""company_id"" AS ""CompanyId"",
                        ""has_access"" AS ""HasAccess"",
                        ""type_id"" AS ""TypeId"",
                        ""last_name"" AS ""LastName""
                        
                    FROM ""representative""
                    OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<Representative>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM ""representative""";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => FromRepresentativeModel(model)).ToList();

                var domainItems = results;

                return Result.Ok(new PaginatedList<Representative>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Representative", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM ""representative"" WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("Representative not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete Representative", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        
        public async Task<List<Representative>> GetByCompanyId(int CompanyId)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""first_name"" AS ""FirstName"",
                        ""second_name"" AS ""SecondName"",
                        ""pin"" AS ""Pin"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""company_id"" AS ""CompanyId"",
                        ""has_access"" AS ""HasAccess"",
                        ""type_id"" AS ""TypeId"",
                        ""last_name"" AS ""LastName""
                        
                    FROM ""representative"" WHERE ""company_id"" = @CompanyId";
                var models = await _dbConnection.QueryAsync<Representative>(sql, new { CompanyId }, transaction: _dbTransaction);
                return models.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get representative by id", ex);
            }
        }
        
        public async Task<List<Representative>> GetByTypeId(int TypeId)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""first_name"" AS ""FirstName"",
                        ""second_name"" AS ""SecondName"",
                        ""pin"" AS ""Pin"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""company_id"" AS ""CompanyId"",
                        ""has_access"" AS ""HasAccess"",
                        ""type_id"" AS ""TypeId"",
                        ""last_name"" AS ""LastName""
                        
                    FROM ""representative"" WHERE ""type_id"" = @TypeId";
                var models = await _dbConnection.QueryAsync<Representative>(sql, new { TypeId }, transaction: _dbTransaction);
                return models.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get representative by id", ex);
            }
        }
        

        private RepresentativeModel ToRepresentativeModel(Representative model)
        {
            return new RepresentativeModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                Pin = model.Pin,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                CompanyId = model.CompanyId,
                HasAccess = model.HasAccess,
                TypeId = model.TypeId,
                LastName = model.LastName
                
            };
        }

        private Representative FromRepresentativeModel(Representative model)
        {
            return new Representative
            {
                Id = model.Id,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                Pin = model.Pin,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                CompanyId = model.CompanyId,
                HasAccess = model.HasAccess,
                TypeId = model.TypeId,
                LastName = model.LastName
                
            };
        }
    }
}
