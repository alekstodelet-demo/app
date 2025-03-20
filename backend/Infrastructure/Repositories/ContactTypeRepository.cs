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
    public class ContactTypeRepository : BaseRepository, IContactTypeRepository
    {
        public ContactTypeRepository(IDbConnection dbConnection, IDbTransaction dbTransaction) 
            : base(dbConnection, dbTransaction)
        {
        }

        public async Task<Result<IEnumerable<ContactType>>> GetAll()
        {
            const string sql = @"
                SELECT id AS ""Id"", 
                       name AS ""Name"", 
                       code AS ""Code"",
                       is_active AS ""IsActive""
                FROM contact_types 
                WHERE is_active = true
                ORDER BY name";

            return await QueryAsync<ContactType>(sql);
        }

        public async Task<Result<ContactType>> GetById(int id)
        {
            const string sql = @"
                SELECT id AS ""Id"", 
                       name AS ""Name"", 
                       code AS ""Code"",
                       is_active AS ""IsActive""
                FROM contact_types 
                WHERE id = @Id";

            return await QuerySingleOrDefaultAsync<ContactType>(sql, new { Id = id });
        }

        public async Task<Result<int>> Add(ContactType entity)
        {
            const string sql = @"
                INSERT INTO contact_types (name, code, is_active, created_at, created_by)
                VALUES (@Name, @Code, @IsActive, @CreatedAt, @CreatedBy)
                RETURNING id";

            var param = new
            {
                entity.Name,
                entity.Code,
                entity.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1 // TODO: Получать из контекста текущего пользователя
            };

            return await QuerySingleOrDefaultAsync<int>(sql, param);
        }

        public async Task<Result> Update(ContactType entity)
        {
            const string sql = @"
                UPDATE contact_types 
                SET name = @Name,
                    code = @Code,
                    is_active = @IsActive,
                    updated_at = @UpdatedAt,
                    updated_by = @UpdatedBy
                WHERE id = @Id";

            var param = new
            {
                entity.Id,
                entity.Name,
                entity.Code,
                entity.IsActive,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1 // TODO: Получать из контекста текущего пользователя
            };

            var result = await ExecuteAsync(sql, param);
            return result.IsSuccess ? Result.Ok() : Result.Fail(result.Errors);
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