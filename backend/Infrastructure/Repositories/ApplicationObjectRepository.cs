using System.Data;
using Dapper;
using Domain.Entities;
using Application.Repositories;
using Infrastructure.Data.Models;
using Application.Exceptions;
using Application.Models;
using System;
using Infrastructure.FillLogData;
using FluentResults;

namespace Infrastructure.Repositories
{
    public class ApplicationObjectRepository : IApplicationObjectRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;

        public ApplicationObjectRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<ApplicationObject>>> GetAll()
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   application_id AS ""ApplicationId"",
                                   arch_object_id AS ""ArchObjectId""
                            FROM application_object";
                var models = await _dbConnection.QueryAsync<ApplicationObject>(sql, transaction: _dbTransaction);
                return Result.Ok(models.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ApplicationObject", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }
        
        public async Task<Result<ApplicationObject>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   application_id AS ""ApplicationId"",
                                   arch_object_id AS ""ArchObjectId""
                            FROM application_object WHERE id = @Id";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<ApplicationObject>(sql, new { Id = id }, 
                    transaction: _dbTransaction);
                
                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"ApplicationObject with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok(model);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ApplicationObject", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(ApplicationObject domain)
        {
            try
            {
                var model = new
                {
                    ApplicationId = domain.ApplicationId,
                    ArchObjectId = domain.ArchObjectId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = 1,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = 1
                };

                var sql = @"INSERT INTO application_object (application_id, arch_object_id, created_at, created_by, updated_at, updated_by) 
                            VALUES (@ApplicationId, @ArchObjectId, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy) 
                            RETURNING id";
                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add ApplicationObject", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(ApplicationObject domain)
        {
            try
            {
                var model = new
                {
                    Id = domain.Id,
                    ApplicationId = domain.ApplicationId,
                    ArchObjectId = domain.ArchObjectId,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = 1
                };

                var sql = @"UPDATE application_object 
                            SET application_id = @ApplicationId, 
                                arch_object_id = @ArchObjectId, 
                                updated_at = @UpdatedAt, 
                                updated_by = @UpdatedBy 
                            WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, model, transaction: _dbTransaction);
                
                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("ApplicationObject not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to update ApplicationObject", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<ApplicationObject>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   application_id AS ""ApplicationId"",
                                   arch_object_id AS ""ArchObjectId""
                            FROM application_object 
                            OFFSET @pageSize * (@pageNumber - 1) LIMIT @pageSize";
                var models = await _dbConnection.QueryAsync<ApplicationObject>(sql, new { pageSize, pageNumber }, 
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT COUNT(*) FROM application_object";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var domainItems = models.ToList();

                return Result.Ok(new PaginatedList<ApplicationObject>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ApplicationObjects", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }
        
        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM application_object WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
                
                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("ApplicationObject not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete ApplicationObject", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        public async Task<Result<List<ApplicationObject>>> GetByApplicationId(int applicationId)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   application_id AS ""ApplicationId"",
                                   arch_object_id AS ""ArchObjectId""
                            FROM application_object 
                            WHERE application_id = @ApplicationId";
                var models = await _dbConnection.QueryAsync<ApplicationObject>(sql, new { ApplicationId = applicationId }, 
                    transaction: _dbTransaction);
                return Result.Ok(models.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ApplicationObject by ApplicationId", ex)
                    .WithMetadata("ErrorCode", "FETCH_BY_APPLICATION_ID_FAILED"));
            }
        }
        
        public async Task<Result<List<ApplicationObject>>> GetByArchObjectId(int archObjectId)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   application_id AS ""ApplicationId"",
                                   arch_object_id AS ""ArchObjectId""
                            FROM application_object 
                            WHERE arch_object_id = @ArchObjectId";
                var models = await _dbConnection.QueryAsync<ApplicationObject>(sql, new { ArchObjectId = archObjectId }, 
                    transaction: _dbTransaction);
                return Result.Ok(models.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ApplicationObject by ArchObjectId", ex)
                    .WithMetadata("ErrorCode", "FETCH_BY_ARCH_OBJECT_ID_FAILED"));
            }
        }
    }
}
