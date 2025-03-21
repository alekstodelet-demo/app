using System.Data;
using Dapper;
using Domain.Entities;
using Application.Repositories;
using Infrastructure.Data.Models;
using Application.Exceptions;
using Application.Models;
using System;
using MySqlX.XDevAPI.Common;

namespace Infrastructure.Repositories
{
    public class RepresentativeContactRepository : IRepresentativeContactRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;

        public RepresentativeContactRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<RepresentativeContact>>> GetAll()
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""value"" AS ""Value"",
                        ""allow_notification"" AS ""AllowNotification"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""r_type_id"" AS ""RTypeId"",
                        ""representative_id"" AS ""RepresentativeId""
                        
                    FROM ""representative_contact"";
                ";

                var models = await _dbConnection.QueryAsync<RepresentativeContact>(sql, transaction: _dbTransaction);

                var results = models.Select(model => FromRepresentativeContactModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get all representative_contact", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<RepresentativeContact>> GetOneByID(int id)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""value"" AS ""Value"",
                        ""allow_notification"" AS ""AllowNotification"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""r_type_id"" AS ""RTypeId"",
                        ""representative_id"" AS ""RepresentativeId""
                        
                    FROM ""representative_contact"" WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<RepresentativeContact>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"RepresentativeContact with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                var result = FromRepresentativeContactModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get RepresentativeContact", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(RepresentativeContact domain)
        {
            try
            {
                var model = ToRepresentativeContactModel(domain);

                var sql = @"
                    INSERT INTO ""representative_contact""(""value"", ""allow_notification"", ""created_at"", ""updated_at"", ""created_by"", ""updated_by"", ""r_type_id"", ""representative_id"" ) 
                    VALUES (@Value, @AllowNotification, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @RTypeId, @RepresentativeId) RETURNING id
                ";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add RepresentativeContact", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(RepresentativeContact domain)
        {
            try
            {
                var model = ToRepresentativeContactModel(domain);

                var sql = @"
                    UPDATE ""representative_contact"" SET 
                    ""id"" = @Id, ""value"" = @Value, ""allow_notification"" = @AllowNotification, ""updated_at"" = @UpdatedAt, ""updated_by"" = @UpdatedBy, ""r_type_id"" = @RTypeId, ""representative_id"" = @RepresentativeId 
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
                return Result.Fail(new ExceptionalError("Failed to update RepresentativeContact", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<RepresentativeContact>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""value"" AS ""Value"",
                        ""allow_notification"" AS ""AllowNotification"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""r_type_id"" AS ""RTypeId"",
                        ""representative_id"" AS ""RepresentativeId""
                        
                    FROM ""representative_contact""
                    OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<RepresentativeContact>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM ""representative_contact""";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => FromRepresentativeContactModel(model)).ToList();

                var domainItems = results;

                return Result.Ok(new PaginatedList<RepresentativeContact>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get RepresentativeContact", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM ""representative_contact"" WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("RepresentativeContact not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete RepresentativeContact", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }


        public async Task<List<RepresentativeContact>> GetByRepresentativeId(int RepresentativeId)
        {
            try
            {
                var sql = @"
                    SELECT 
                        ""id"" AS ""Id"",
                        ""value"" AS ""Value"",
                        ""allow_notification"" AS ""AllowNotification"",
                        ""created_at"" AS ""CreatedAt"",
                        ""updated_at"" AS ""UpdatedAt"",
                        ""created_by"" AS ""CreatedBy"",
                        ""updated_by"" AS ""UpdatedBy"",
                        ""r_type_id"" AS ""RTypeId"",
                        ""representative_id"" AS ""RepresentativeId""
                        
                    FROM ""representative_contact"" WHERE ""representative_id"" = @RepresentativeId";
                var models = await _dbConnection.QueryAsync<RepresentativeContact>(sql, new { RepresentativeId }, transaction: _dbTransaction);
                return models.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get representative_contact by id", ex);
            }
        }


        private RepresentativeContactModel ToRepresentativeContactModel(RepresentativeContact model)
        {
            return new RepresentativeContactModel
            {
                Id = model.Id,
                Value = model.Value,
                AllowNotification = model.AllowNotification,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                RTypeId = model.RTypeId,
                RepresentativeId = model.RepresentativeId

            };
        }

        private RepresentativeContact FromRepresentativeContactModel(RepresentativeContact model)
        {
            return new RepresentativeContact
            {
                Id = model.Id,
                Value = model.Value,
                AllowNotification = model.AllowNotification,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                RTypeId = model.RTypeId,
                RepresentativeId = model.RepresentativeId

            };
        }
    }
}
