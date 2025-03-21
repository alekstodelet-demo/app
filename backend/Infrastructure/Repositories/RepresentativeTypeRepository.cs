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
    public class RepresentativeTypeRepository : IRepresentativeTypeRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;

        public RepresentativeTypeRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<RepresentativeType>>> GetAll()
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""description"" AS ""Description"",
                        ""name"" AS ""Name"",
                        ""code"" AS ""Code""
                        
                    FROM ""representative_type"";
                ";

                var models = await _dbConnection.QueryAsync<RepresentativeType>(sql, transaction: _dbTransaction);
                
                var results = models.Select(model => FromRepresentativeTypeModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get all representative_type", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<RepresentativeType>> GetOneByID(int id)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""description"" AS ""Description"",
                        ""name"" AS ""Name"",
                        ""code"" AS ""Code""
                        
                    FROM ""representative_type"" WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<RepresentativeType>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"RepresentativeType with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                var result = FromRepresentativeTypeModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get RepresentativeType", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(RepresentativeType domain)
        {
            try
            {
                var model = ToRepresentativeTypeModel(domain);

                var sql = @"
                    INSERT INTO ""representative_type""(""created_at"", ""updated_at"", ""created_by"", ""updated_by"", ""description"", ""name"", ""code"" ) 
                    VALUES (@CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @Description, @Name, @Code) RETURNING id
                ";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add RepresentativeType", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(RepresentativeType domain)
        {
            try
            {
                var model = ToRepresentativeTypeModel(domain);

                var sql = @"
                    UPDATE ""representative_type"" SET 
                    ""id"" = @Id, ""updated_at"" = @UpdatedAt, ""updated_by"" = @UpdatedBy, ""description"" = @Description, ""name"" = @Name, ""code"" = @Code 
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
                return Result.Fail(new ExceptionalError("Failed to update RepresentativeType", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<RepresentativeType>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""description"" AS ""Description"",
                        ""name"" AS ""Name"",
                        ""code"" AS ""Code""
                        
                    FROM ""representative_type""
                    OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<RepresentativeType>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM ""representative_type""";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => FromRepresentativeTypeModel(model)).ToList();

                var domainItems = results;

                return Result.Ok(new PaginatedList<RepresentativeType>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get RepresentativeType", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM ""representative_type"" WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("RepresentativeType not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete RepresentativeType", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        

        private RepresentativeTypeModel ToRepresentativeTypeModel(RepresentativeType model)
        {
            return new RepresentativeTypeModel
            {
                Id = model.Id,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                Description = model.Description,
                Name = model.Name,
                Code = model.Code
                
            };
        }

        private RepresentativeType FromRepresentativeTypeModel(RepresentativeType model)
        {
            return new RepresentativeType
            {
                Id = model.Id,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                Description = model.Description,
                Name = model.Name,
                Code = model.Code
                
            };
        }
    }
}
