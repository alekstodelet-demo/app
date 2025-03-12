using System.Data;
using Dapper;
using Domain.Entities;
using Application.Repositories;
using Application.Models;
using FluentResults;
using System;
using Infrastructure.FillLogData;

namespace Infrastructure.Repositories
{
    public class ArchObjectRepository : IArchObjectRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;

        public ArchObjectRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<ArchObject>>> GetAll()
        {
            try
            {
                var sql = @"SELECT ao.id AS ""Id"", 
                                   address AS ""Address"", 
                                   ao.name AS ""Name"", 
                                   identifier AS ""Identifier"", 
                                   district_id AS ""DistrictId"", 
                                   ao.description AS ""Description"", 
                                   xcoordinate AS ""XCoordinate"", 
                                   ycoordinate AS ""YCoordinate""
                            FROM arch_object ao";
                var models = await _dbConnection.QueryAsync<ArchObject>(sql, transaction: _dbTransaction);
                return Result.Ok(models.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ArchObjects", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<List<ArchObject>>> GetBySearch(string text)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"", 
                                   address AS ""Address"", 
                                   name AS ""Name"", 
                                   identifier AS ""Identifier"", 
                                   district_id AS ""DistrictId"", 
                                   description AS ""Description"", 
                                   xcoordinate AS ""XCoordinate"", 
                                   ycoordinate AS ""YCoordinate""
                            FROM arch_object";
                
                if (!string.IsNullOrEmpty(text))
                {
                    sql += @" WHERE LOWER(name) LIKE LOWER(@Text) 
                              OR LOWER(address) LIKE LOWER(@Text) 
                              OR LOWER(description) LIKE LOWER(@Text)";
                }

                sql += @" LIMIT 50";
                var models = await _dbConnection.QueryAsync<ArchObject>(sql, new { Text = $"%{text}%" }, 
                    transaction: _dbTransaction);
                return Result.Ok(models.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ArchObjects by search", ex)
                    .WithMetadata("ErrorCode", "FETCH_BY_SEARCH_FAILED"));
            }
        }

        public async Task<Result<List<ArchObject>>> GetByApplicationId(int applicationId)
        {
            try
            {
                var sql = @"
                            SELECT obj.id AS ""Id"", 
                                   obj.address AS ""Address"", 
                                   obj.name AS ""Name"", 
                                   obj.identifier AS ""Identifier"", 
                                   obj.district_id AS ""DistrictId"", 
                                   obj.description AS ""Description"", 
                                   obj.xcoordinate AS ""XCoordinate"", 
                                   obj.ycoordinate AS ""YCoordinate""
                            FROM application_object ao
                            LEFT JOIN arch_object obj ON obj.id = ao.arch_object_id
                            WHERE ao.application_id = @ApplicationId
                            GROUP BY obj.id, dis.name";

                var models = await _dbConnection.QueryAsync<ArchObject>(sql, new { ApplicationId = applicationId }, 
                    transaction: _dbTransaction);
                return Result.Ok(models.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ArchObjects by application ID", ex)
                    .WithMetadata("ErrorCode", "FETCH_BY_APP_ID_FAILED"));
            }
        }

        public async Task<Result<ArchObject>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT ao.id AS ""Id"", 
                                   address AS ""Address"", 
                                   ao.name AS ""Name"", 
                                   identifier AS ""Identifier"", 
                                   district_id AS ""DistrictId"", 
                                   ao.description AS ""Description"", 
                                   xcoordinate AS ""XCoordinate"", 
                                   ycoordinate AS ""YCoordinate""
                            FROM arch_object ao 
                            WHERE ao.id = @Id";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<ArchObject>(sql, new { Id = id }, 
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"ArchObject with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok(model);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ArchObject", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(ArchObject domain)
        {
            try
            {
                var model = new
                {
                    Address = domain.Address,
                    Name = domain.Name,
                    Identifier = domain.Identifier,
                    DistrictId = domain.DistrictId,
                    Description = domain.Description,
                    XCoordinate = domain.XCoordinate,
                    YCoordinate = domain.YCoordinate,
                    CreatedAt = DateTime.Now,
                    CreatedBy = 1,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = 1
                };

                var sql = @"INSERT INTO arch_object (address, name, identifier, district_id, created_at, description, 
                                                    xcoordinate, ycoordinate, updated_at, created_by, updated_by) 
                            VALUES (@Address, @Name, @Identifier, @DistrictId, @CreatedAt, @Description, 
                                    @XCoordinate, @YCoordinate, @UpdatedAt, @CreatedBy, @UpdatedBy) 
                            RETURNING id";
                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add ArchObject", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(ArchObject domain)
        {
            try
            {
                var model = new
                {
                    Id = domain.Id,
                    Address = domain.Address,
                    Name = domain.Name,
                    Identifier = domain.Identifier,
                    DistrictId = domain.DistrictId,
                    Description = domain.Description,
                    XCoordinate = domain.XCoordinate,
                    YCoordinate = domain.YCoordinate,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = 1,
                };

                var sql = @"UPDATE arch_object 
                            SET address = @Address, 
                                name = @Name, 
                                identifier = @Identifier, 
                                district_id = @DistrictId, 
                                updated_at = @UpdatedAt, 
                                description = @Description, 
                                xcoordinate = @XCoordinate, 
                                ycoordinate = @YCoordinate, 
                                updated_by = @UpdatedBy 
                            WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, model, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("ArchObject not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to update ArchObject", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<ArchObject>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"", 
                                   address AS ""Address"", 
                                   name AS ""Name"", 
                                   identifier AS ""Identifier"", 
                                   district_id AS ""DistrictId"", 
                                   description AS ""Description"", 
                                   xcoordinate AS ""XCoordinate"", 
                                   ycoordinate AS ""YCoordinate""
                            FROM arch_object 
                            OFFSET @pageSize * (@pageNumber - 1) 
                            LIMIT @pageSize";
                var models = await _dbConnection.QueryAsync<ArchObject>(sql, new { pageSize, pageNumber }, 
                    transaction: _dbTransaction);

                var sqlCount = "SELECT COUNT(*) FROM arch_object";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var domainItems = models.ToList();
                return Result.Ok(new PaginatedList<ArchObject>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get paginated ArchObjects", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = "DELETE FROM arch_object WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("ArchObject not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete ArchObject", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }
    }
}