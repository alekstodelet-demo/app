using System.Data;
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
    public class OrganizationTypeRepository : IOrganizationTypeRepository
    {
        private IDbTransaction? _dbTransaction;
        private IDbConnection _dbConnection;

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
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   code AS ""Code"",
                                   description AS ""Description"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   is_active AS ""IsActive""
                            FROM c_organization_type
                            ORDER BY name";
                
                var models = await _dbConnection.QueryAsync<OrganizationType>(sql, transaction: _dbTransaction);
                return Result.Ok(models.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get OrganizationTypes", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<OrganizationType>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   code AS ""Code"",
                                   description AS ""Description"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   is_active AS ""IsActive""
                            FROM c_organization_type
                            WHERE id = @Id";
                
                var model = await _dbConnection.QuerySingleOrDefaultAsync<OrganizationType>(
                    sql, new { Id = id }, transaction: _dbTransaction);
                
                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"OrganizationType with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                
                return Result.Ok(model);
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
                domain.CreatedAt = DateTime.Now;
                domain.CreatedBy = 1; // This should be the current user ID in a real scenario
                domain.UpdatedAt = DateTime.Now;
                domain.UpdatedBy = 1;

                var sql = @"INSERT INTO c_organization_type(
                                name,
                                code,
                                description,
                                created_at,
                                updated_at,
                                created_by,
                                updated_by,
                                is_active)
                            VALUES (
                                @Name,
                                @Code,
                                @Description,
                                @CreatedAt,
                                @UpdatedAt,
                                @CreatedBy,
                                @UpdatedBy,
                                @IsActive)
                            RETURNING id";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, domain, transaction: _dbTransaction);
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
                domain.UpdatedAt = DateTime.Now;
                domain.UpdatedBy = 1; // This should be the current user ID in a real scenario

                var sql = @"UPDATE c_organization_type SET 
                                name = @Name,
                                code = @Code,
                                description = @Description,
                                updated_at = @UpdatedAt,
                                updated_by = @UpdatedBy,
                                is_active = @IsActive
                            WHERE id = @Id";

                var affected = await _dbConnection.ExecuteAsync(sql, domain, transaction: _dbTransaction);
                
                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("OrganizationType not found", null)
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
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   code AS ""Code"",
                                   description AS ""Description"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   is_active AS ""IsActive""
                            FROM c_organization_type
                            ORDER BY name
                            OFFSET @Offset LIMIT @PageSize";

                var offset = (pageNumber - 1) * pageSize;
                var models = await _dbConnection.QueryAsync<OrganizationType>(
                    sql,
                    new { Offset = offset, PageSize = pageSize },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT COUNT(*) FROM c_organization_type";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                return Result.Ok(new PaginatedList<OrganizationType>(models.ToList(), totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get paginated OrganizationTypes", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM c_organization_type WHERE id = @Id";
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

        public async Task<Result<OrganizationType>> GetByCode(string code)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   code AS ""Code"",
                                   description AS ""Description"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   is_active AS ""IsActive""
                            FROM c_organization_type
                            WHERE code = @Code";
                
                var model = await _dbConnection.QuerySingleOrDefaultAsync<OrganizationType>(
                    sql, new { Code = code }, transaction: _dbTransaction);
                
                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"OrganizationType with code {code} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                
                return Result.Ok(model);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get OrganizationType by code", ex)
                    .WithMetadata("ErrorCode", "FETCH_BY_CODE_FAILED"));
            }
        }
    }
}