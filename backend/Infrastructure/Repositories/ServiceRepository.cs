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
    public class ServiceRepository : IServiceRepository
    {
        private IDbTransaction? _dbTransaction;
        private IDbConnection _dbConnection;
        private EncryptionService _crypt;

        public ServiceRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _crypt = new EncryptionService(configuration);
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<Service>>> GetAll()
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   short_name AS ""ShortName"",
                                   code AS ""Code"",
                                   description AS ""Description"",
                                   day_count AS ""DayCount"",
                                   workflow_id AS ""WorkflowId"",
                                   price AS ""Price"",
                                   name_kg AS ""NameKg"",
                                   name_long AS ""NameLong"",
                                   name_long_kg AS ""NameLongKg"",
                                   name_statement AS ""NameStatement"",
                                   name_statement_kg AS ""NameStatementKg"",
                                   name_confirmation AS ""NameConfirmation"",
                                   name_confirmation_kg AS ""NameConfirmationKg"",
                                   is_active AS ""IsActive"",
                                   created_at AS ""CreatedAt"",
                                   created_by AS ""CreatedBy"", 
                                   updated_at AS ""UpdatedAt"", 
                                   updated_by AS ""UpdatedBy"", 
                                   description_kg AS ""DescriptionKg"", 
                                   text_color AS ""TextColor"", 
                                   background_color AS ""BackgroundColor"" 
                            FROM service;";
                var models = await _dbConnection.QueryAsync<Service>(sql, transaction: _dbTransaction);
        
                var results = models.Select(model => DecryptServiceModel(model)).ToList();
                
                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Service", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<Service>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   short_name AS ""ShortName"",
                                   code AS ""Code"",
                                   description AS ""Description"",
                                   day_count AS ""DayCount"",
                                   workflow_id AS ""WorkflowId"",
                                   price AS ""Price"",
                                   name_kg AS ""NameKg"",
                                   name_long AS ""NameLong"",
                                   name_long_kg AS ""NameLongKg"",
                                   name_statement AS ""NameStatement"",
                                   name_statement_kg AS ""NameStatementKg"",
                                   name_confirmation AS ""NameConfirmation"",
                                   name_confirmation_kg AS ""NameConfirmationKg"",
                                   is_active AS ""IsActive"",
                                   created_at AS ""CreatedAt"",
                                   created_by AS ""CreatedBy"", 
                                   updated_at AS ""UpdatedAt"", 
                                   updated_by AS ""UpdatedBy"", 
                                   description_kg AS ""DescriptionKg"", 
                                   text_color AS ""TextColor"", 
                                   background_color AS ""BackgroundColor"" 
                            FROM service WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<Service>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"Service with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                
                var result = DecryptServiceModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Service", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(Service domain)
        {
            try
            {
                var model = EncryptServiceModel(domain);
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = 1;
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;
                
                var sql = @"INSERT INTO service(name, short_name, code, description, day_count, workflow_id,
                                price, created_at, updated_at, created_by, updated_by) 
                    VALUES (@Name, @ShortName, @Code, @Description, @DayCount, @WorkflowId, @Price,
                            @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy) RETURNING id";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add method error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Result.Fail(new ExceptionalError("Failed to add Service", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(Service domain)
        {
            try
            {
                var model = EncryptServiceModel(domain);
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;

                var sql = "UPDATE service SET name = @Name, short_name = @ShortName, code = @Code," +
                          " description = @Description, day_count = @DayCount, workflow_id = @WorkflowId, " +
                          "price = @Price, updated_at = @UpdatedAt, updated_by = @UpdatedBy WHERE id = @Id";
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
                return Result.Fail(new ExceptionalError("Failed to update Service", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<Service>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   short_name AS ""ShortName"",
                                   code AS ""Code"",
                                   description AS ""Description"",
                                   day_count AS ""DayCount"",
                                   workflow_id AS ""WorkflowId"",
                                   price AS ""Price"",
                                   name_kg AS ""NameKg"",
                                   name_long AS ""NameLong"",
                                   name_long_kg AS ""NameLongKg"",
                                   name_statement AS ""NameStatement"",
                                   name_statement_kg AS ""NameStatementKg"",
                                   name_confirmation AS ""NameConfirmation"",
                                   name_confirmation_kg AS ""NameConfirmationKg"",
                                   is_active AS ""IsActive"",
                                   created_at AS ""CreatedAt"",
                                   created_by AS ""CreatedBy"", 
                                   updated_at AS ""UpdatedAt"", 
                                   updated_by AS ""UpdatedBy"", 
                                   description_kg AS ""DescriptionKg"", 
                                   text_color AS ""TextColor"", 
                                   background_color AS ""BackgroundColor"" 
                            FROM service
                            OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<Service>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM service";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => DecryptServiceModel(model)).ToList();

                var domainItems = results;

                return Result.Ok(new PaginatedList<Service>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Service", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM service WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("Service not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete Service", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }
        
        private ServiceModel EncryptServiceModel(Service model)
        {
            return new ServiceModel
            {
                Id = model.Id,
                Name = _crypt.Encrypt(model.Name), // Шифруем Name
                ShortName = model.ShortName,
                Code = model.Code,
                Description = model.Description,
                DayCount = model.DayCount,
                WorkflowId = model.WorkflowId,
                Price = model.Price
            };
        }
        
        private Service DecryptServiceModel(Service model)
        {
            return new Service
            {
                Id = model.Id,
                Name = _crypt.Decrypt(model.Name), // Расшифровываем Name
                ShortName = model.ShortName,
                Code = model.Code,
                Description = model.Description,
                DayCount = model.DayCount,
                WorkflowId = model.WorkflowId,
                Price = model.Price
            };
        }
    }
}