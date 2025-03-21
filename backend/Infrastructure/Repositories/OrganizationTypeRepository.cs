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
    public class OrganizationTypeRepository : IOrganizationTypeRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;

        public OrganizationTypeRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<OrganizationType>>> GetAll()
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""description_kg"" AS ""DescriptionKg"",
                        ""text_color"" AS ""TextColor"",
                        ""background_color"" AS ""BackgroundColor"",
                        ""name"" AS ""Name"",
                        ""description"" AS ""Description"",
                        ""code"" AS ""Code"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""name_kg"" AS ""NameKg""
                        
                    FROM ""organization_type"";
                ";

                var models = await _dbConnection.QueryAsync<OrganizationType>(sql, transaction: _dbTransaction);
                
                var results = models.Select(model => FromOrganizationTypeModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get all organization_type", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<OrganizationType>> GetOneByID(int id)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""description_kg"" AS ""DescriptionKg"",
                        ""text_color"" AS ""TextColor"",
                        ""background_color"" AS ""BackgroundColor"",
                        ""name"" AS ""Name"",
                        ""description"" AS ""Description"",
                        ""code"" AS ""Code"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""name_kg"" AS ""NameKg""
                        
                    FROM ""organization_type"" WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<OrganizationType>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"OrganizationType with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                var result = FromOrganizationTypeModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get OrganizationType", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(OrganizationType domain)
        {
            try
            {
                var model = ToOrganizationTypeModel(domain);

                var sql = @"
                    INSERT INTO ""organization_type""(""description_kg"", ""text_color"", ""background_color"", ""name"", ""description"", ""code"", ""created_at"", ""updated_at"", ""created_by"", ""updated_by"", ""name_kg"" ) 
                    VALUES (@DescriptionKg, @TextColor, @BackgroundColor, @Name, @Description, @Code, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @NameKg) RETURNING id
                ";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add OrganizationType", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(OrganizationType domain)
        {
            try
            {
                var model = ToOrganizationTypeModel(domain);

                var sql = @"
                    UPDATE ""organization_type"" SET 
                    ""id"" = @Id, ""description_kg"" = @DescriptionKg, ""text_color"" = @TextColor, ""background_color"" = @BackgroundColor, ""name"" = @Name, ""description"" = @Description, ""code"" = @Code, ""updated_at"" = @UpdatedAt, ""updated_by"" = @UpdatedBy, ""name_kg"" = @NameKg 
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
                return Result.Fail(new ExceptionalError("Failed to update OrganizationType", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<OrganizationType>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""description_kg"" AS ""DescriptionKg"",
                        ""text_color"" AS ""TextColor"",
                        ""background_color"" AS ""BackgroundColor"",
                        ""name"" AS ""Name"",
                        ""description"" AS ""Description"",
                        ""code"" AS ""Code"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""name_kg"" AS ""NameKg""
                        
                    FROM ""organization_type""
                    OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<OrganizationType>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM ""organization_type""";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => FromOrganizationTypeModel(model)).ToList();

                var domainItems = results;

                return Result.Ok(new PaginatedList<OrganizationType>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get OrganizationType", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM ""organization_type"" WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("OrganizationType not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete OrganizationType", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        

        private OrganizationTypeModel ToOrganizationTypeModel(OrganizationType model)
        {
            return new OrganizationTypeModel
            {
                Id = model.Id,
                DescriptionKg = model.DescriptionKg,
                TextColor = model.TextColor,
                BackgroundColor = model.BackgroundColor,
                Name = model.Name,
                Description = model.Description,
                Code = model.Code,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                NameKg = model.NameKg
                
            };
        }

        private OrganizationType FromOrganizationTypeModel(OrganizationType model)
        {
            return new OrganizationType
            {
                Id = model.Id,
                DescriptionKg = model.DescriptionKg,
                TextColor = model.TextColor,
                BackgroundColor = model.BackgroundColor,
                Name = model.Name,
                Description = model.Description,
                Code = model.Code,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                NameKg = model.NameKg
                
            };
        }
    }
}
