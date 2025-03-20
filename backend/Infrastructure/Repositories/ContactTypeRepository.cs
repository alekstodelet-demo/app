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
    public class ContactTypeRepository : IContactTypeRepository
    {
        private IDbTransaction? _dbTransaction;
        private IDbConnection _dbConnection;
        private EncryptionService _crypt;

        public ContactTypeRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _crypt = new EncryptionService(configuration);
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<ContactType>>> GetAll()
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   code AS ""Code"",
                                   description AS ""Description""
                            FROM contact_type;";
                var models = await _dbConnection.QueryAsync<ContactType>(sql, transaction: _dbTransaction);

                var results = models.Select(model => FromContactTypeModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ContactType", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<ContactType>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   code AS ""Code"",
                                   description AS ""Description""
                            FROM contact_type WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<ContactType>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"ContactType with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                var result = FromContactTypeModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ContactType", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(ContactType domain)
        {
            try
            {
                var model = ToContactTypeModel(domain);
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = 1;
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;

                var sql = @"INSERT INTO customer_contact(name, code, description, created_at, updated_at, created_by, updated_by) 
                    VALUES (@Name, @Code, @Decsription,
                            @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy) RETURNING id";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add method error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Result.Fail(new ExceptionalError("Failed to add ContactType", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(ContactType domain)
        {
            try
            {
                var model = ToContactTypeModel(domain);
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;

                var sql = @"UPDATE customer_contact SET name = @Name, code = @Code, description = @description
                          updated_at = @UpdatedAt, updated_by = @UpdatedBy WHERE id = @Id";
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
                return Result.Fail(new ExceptionalError("Failed to update ContactType", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<ContactType>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   code AS ""Code"",
                                   description AS ""Description""
                            FROM customer_contact
                            OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<ContactType>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM customer_contact";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => FromContactTypeModel(model)).ToList();

                var domainItems = results;

                return Result.Ok(new PaginatedList<ContactType>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get ContactType", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM customer_contact WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("ContactType not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete ContactType", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        private ContactTypeModel ToContactTypeModel(ContactType model)
        {
            return new ContactTypeModel
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code,
                Description = model.Description,
            };
        }

        private ContactType FromContactTypeModel(ContactType model)
        {
            return new ContactType
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code,
                Description = model.Description,
            };
        }
    }
}