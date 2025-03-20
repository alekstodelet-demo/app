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
    public class OrganizationRepository : IOrganizationRepository
    {
        private IDbTransaction? _dbTransaction;
        private IDbConnection _dbConnection;
        private EncryptionService _crypt;

        public OrganizationRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _crypt = new EncryptionService(configuration);
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<Organization>>> GetAll()
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   short_name AS ""ShortName"",
                                   inn AS ""Inn"",
                                   director AS ""Director"",
                                   address AS ""Address"",
                                   email AS ""Email"",
                                   phone AS ""Phone"",
                                   organization_type_id AS ""OrganizationTypeId"",
                                   ot.name AS ""OrganizationTypeName"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   website AS ""Website"",
                                   description AS ""Description"",
                                   is_active AS ""IsActive""
                            FROM c_organization o
                            LEFT JOIN c_organization_type ot ON o.organization_type_id = ot.id";
                var models = await _dbConnection.QueryAsync<OrganizationModel>(sql, transaction: _dbTransaction);

                // Apply any needed decryption or transformations
                var results = models.Select(model => DecryptOrganizationModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Organizations", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<Organization>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   short_name AS ""ShortName"",
                                   inn AS ""Inn"",
                                   director AS ""Director"",
                                   address AS ""Address"",
                                   email AS ""Email"",
                                   phone AS ""Phone"",
                                   organization_type_id AS ""OrganizationTypeId"",
                                   ot.name AS ""OrganizationTypeName"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   website AS ""Website"",
                                   description AS ""Description"",
                                   is_active AS ""IsActive""
                            FROM c_organization o
                            LEFT JOIN c_organization_type ot ON o.organization_type_id = ot.id
                            WHERE o.id = @Id";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<OrganizationModel>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"Organization with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                // Apply any needed decryption or transformations
                var result = DecryptOrganizationModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Organization", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(Organization domain)
        {
            try
            {
                // Apply any needed encryption or transformations
                var model = EncryptOrganizationModel(domain);
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = 1; // This should be the current user ID in a real scenario
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;

                var sql = @"INSERT INTO c_organization(
                                name,
                                short_name,
                                inn,
                                director,
                                address,
                                email,
                                phone,
                                organization_type_id,
                                created_at,
                                updated_at,
                                created_by,
                                updated_by,
                                website,
                                description,
                                is_active)
                            VALUES (
                                @Name,
                                @ShortName,
                                @Inn,
                                @Director,
                                @Address,
                                @Email,
                                @Phone,
                                @OrganizationTypeId,
                                @CreatedAt,
                                @UpdatedAt,
                                @CreatedBy,
                                @UpdatedBy,
                                @Website,
                                @Description,
                                @IsActive)
                            RETURNING id";

                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add Organization", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(Organization domain)
        {
            try
            {
                // Apply any needed encryption or transformations
                var model = EncryptOrganizationModel(domain);
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1; // This should be the current user ID in a real scenario

                var sql = @"UPDATE c_organization SET 
                                name = @Name,
                                short_name = @ShortName,
                                inn = @Inn,
                                director = @Director,
                                address = @Address,
                                email = @Email,
                                phone = @Phone,
                                organization_type_id = @OrganizationTypeId,
                                updated_at = @UpdatedAt,
                                updated_by = @UpdatedBy,
                                website = @Website,
                                description = @Description,
                                is_active = @IsActive
                            WHERE id = @Id";

                var affected = await _dbConnection.ExecuteAsync(sql, model, transaction: _dbTransaction);
                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("Organization not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to update Organization", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<Organization>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   short_name AS ""ShortName"",
                                   inn AS ""Inn"",
                                   director AS ""Director"",
                                   address AS ""Address"",
                                   email AS ""Email"",
                                   phone AS ""Phone"",
                                   organization_type_id AS ""OrganizationTypeId"",
                                   ot.name AS ""OrganizationTypeName"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   website AS ""Website"",
                                   description AS ""Description"",
                                   is_active AS ""IsActive""
                            FROM c_organization o
                            LEFT JOIN c_organization_type ot ON o.organization_type_id = ot.id
                            ORDER BY o.id
                            OFFSET @Offset LIMIT @PageSize";

                var offset = (pageNumber - 1) * pageSize;
                var models = await _dbConnection.QueryAsync<OrganizationModel>(
                    sql,
                    new { Offset = offset, PageSize = pageSize },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT COUNT(*) FROM c_organization";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                // Apply any needed decryption or transformations
                var results = models.Select(model => DecryptOrganizationModel(model)).ToList();

                return Result.Ok(new PaginatedList<Organization>(results, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get paginated Organizations", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM c_organization WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("Organization not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete Organization", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        public async Task<Result<List<Organization>>> FindByName(string name)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   short_name AS ""ShortName"",
                                   inn AS ""Inn"",
                                   director AS ""Director"",
                                   address AS ""Address"",
                                   email AS ""Email"",
                                   phone AS ""Phone"",
                                   organization_type_id AS ""OrganizationTypeId"",
                                   ot.name AS ""OrganizationTypeName"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   website AS ""Website"",
                                   description AS ""Description"",
                                   is_active AS ""IsActive""
                            FROM c_organization o
                            LEFT JOIN c_organization_type ot ON o.organization_type_id = ot.id
                            WHERE LOWER(o.name) LIKE LOWER(@SearchTerm) OR LOWER(o.short_name) LIKE LOWER(@SearchTerm)";

                var searchTerm = $"%{name}%";
                var models = await _dbConnection.QueryAsync<OrganizationModel>(
                    sql,
                    new { SearchTerm = searchTerm },
                    transaction: _dbTransaction);

                // Apply any needed decryption or transformations
                var results = models.Select(model => DecryptOrganizationModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to find Organizations by name", ex)
                    .WithMetadata("ErrorCode", "FIND_BY_NAME_FAILED"));
            }
        }

        public async Task<Result<Organization>> FindByInn(string inn)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   name AS ""Name"",
                                   short_name AS ""ShortName"",
                                   inn AS ""Inn"",
                                   director AS ""Director"",
                                   address AS ""Address"",
                                   email AS ""Email"",
                                   phone AS ""Phone"",
                                   organization_type_id AS ""OrganizationTypeId"",
                                   ot.name AS ""OrganizationTypeName"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   website AS ""Website"",
                                   description AS ""Description"",
                                   is_active AS ""IsActive""
                            FROM c_organization o
                            LEFT JOIN c_organization_type ot ON o.organization_type_id = ot.id
                            WHERE o.inn = @Inn";

                var model = await _dbConnection.QuerySingleOrDefaultAsync<OrganizationModel>(
                    sql,
                    new { Inn = inn },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"Organization with INN {inn} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                // Apply any needed decryption or transformations
                var result = DecryptOrganizationModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to find Organization by INN", ex)
                    .WithMetadata("ErrorCode", "FIND_BY_INN_FAILED"));
            }
        }

        // Helper methods for encryption/decryption
        private OrganizationModel EncryptOrganizationModel(Organization entity)
        {
            return new OrganizationModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ShortName = entity.ShortName,
                Inn = entity.Inn,
                Director = entity.Director,
                Address = entity.Address,
                Email = _crypt.Encrypt(entity.Email), // Encrypting sensitive data
                Phone = _crypt.Encrypt(entity.Phone), // Encrypting sensitive data
                OrganizationTypeId = entity.OrganizationTypeId,
                OrganizationTypeName = entity.OrganizationTypeName,
                Website = entity.Website,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy
            };
        }

        private Organization DecryptOrganizationModel(OrganizationModel model)
        {
            return new Organization
            {
                Id = model.Id,
                Name = model.Name,
                ShortName = model.ShortName,
                Inn = model.Inn,
                Director = model.Director,
                Address = model.Address,
                Email = _crypt.Decrypt(model.Email), // Decrypting sensitive data
                Phone = _crypt.Decrypt(model.Phone), // Decrypting sensitive data
                OrganizationTypeId = model.OrganizationTypeId,
                OrganizationTypeName = model.OrganizationTypeName,
                Website = model.Website,
                Description = model.Description,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedAt = model.UpdatedAt,
                UpdatedBy = model.UpdatedBy
            };
        }
    }
}