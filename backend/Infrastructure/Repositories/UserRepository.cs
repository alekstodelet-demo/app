using System.Data;
using Application.Models;
using Application.Repositories;
using Dapper;
using DocumentFormat.OpenXml.EMMA;
using Domain.Entities;
using FluentResults;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IConfiguration _configuration;
    private IDbTransaction? _dbTransaction;
    private readonly EncryptionService _crypt;

    public UserRepository(IDbConnection dbConnection, IConfiguration configuration)
    {
        _dbConnection = dbConnection;
        _configuration = configuration;
        _crypt = new EncryptionService(configuration);
    }

    public void SetTransaction(IDbTransaction dbTransaction)
    {
        _dbTransaction = dbTransaction;
    }

    private UserModel EncryptUserModel(User domain)
    {
        return new UserModel
        {
            Id = domain.Id,
            Email = _crypt.Encrypt(domain.Email),
            Phone = _crypt.Encrypt(domain.Phone),
            FirstName = domain.FirstName,
            LastName = domain.LastName,
            TwoFactorEnabled = domain.TwoFactorEnabled,
            TwoFactorSecret = domain.TwoFactorSecret,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt,
            CreatedBy = domain.CreatedBy,
            UpdatedBy = domain.UpdatedBy,
            HashCode = domain.HashCode,
            HashCodeDate = domain.HashCodeDate,
            SecondName = domain.SecondName,
            Pin = domain.Pin,
            ContractSigned = domain.ContractSigned,
            ContractDate = domain.ContractDate,
            PasswordHash = domain.PasswordHash
        };
    }

    private User DecryptUserModel(UserModel model)
    {
        return new User
        {
            Id = model.Id,
            Email = _crypt.Decrypt(model.Email),
            Phone = _crypt.Decrypt(model.Phone),
            FirstName = model.FirstName,
            LastName = model.LastName,
            TwoFactorEnabled = model.TwoFactorEnabled,
            TwoFactorSecret = model.TwoFactorSecret,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            CreatedBy = model.CreatedBy,
            UpdatedBy = model.UpdatedBy,
            HashCode = model.HashCode,
            HashCodeDate = model.HashCodeDate,
            SecondName = model.SecondName,
            Pin = model.Pin,
            ContractSigned = model.ContractSigned,
            ContractDate = model.ContractDate
        };
    }

    public async Task<Result<List<User>>> GetAll()
    {
        try
        {
            var sql = @"SELECT id AS ""Id"", 
                               email AS ""Email"", 
                               phone AS ""Phone"", 
                               first_name AS ""FirstName"", 
                               last_name AS ""LastName"",
                               two_factor_enabled AS ""TwoFactorEnabled"",
                               two_factor_secret AS ""TwoFactorSecret""
                        FROM ""User""";
            
            var models = await _dbConnection.QueryAsync<UserModel>(sql, transaction: _dbTransaction);
            
            var users = models.Select(DecryptUserModel).ToList();
            
            return Result.Ok(users);
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError("Failed to get Users", ex)
                .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
        }
    }

    public async Task<Result<User>> GetOneByID(int id)
    {
        try
        {
            var sql = @"SELECT id AS ""Id"", 
                               email AS ""Email"", 
                               phone AS ""Phone"", 
                               first_name AS ""FirstName"", 
                               last_name AS ""LastName"",
                               two_factor_enabled AS ""TwoFactorEnabled"",
                               two_factor_secret AS ""TwoFactorSecret""
                        FROM ""User"" WHERE id = @Id";
            
            var model = await _dbConnection.QuerySingleOrDefaultAsync<UserModel>(
                sql, new { Id = id }, transaction: _dbTransaction);
            
            if (model == null)
            {
                return Result.Fail(new ExceptionalError($"User with ID {id} not found", null)
                    .WithMetadata("ErrorCode", "NOT_FOUND"));
            }
            
            return Result.Ok(DecryptUserModel(model));
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError("Failed to get User", ex)
                .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
        }
    }

    public async Task<Result<User>> GetByEmail(string email)
    {
        try
        {
            var encryptedEmail = _crypt.Encrypt(email);
            
            var sql = @"SELECT id AS ""Id"", 
                               email AS ""Email"", 
                               phone AS ""Phone"", 
                               first_name AS ""FirstName"", 
                               last_name AS ""LastName"",
                               two_factor_enabled AS ""TwoFactorEnabled"",
                               two_factor_secret AS ""TwoFactorSecret""
                        FROM ""User"" WHERE email = @Email";
            
            var model = await _dbConnection.QuerySingleOrDefaultAsync<UserModel>(
                sql, new { Email = encryptedEmail }, transaction: _dbTransaction);
            
            if (model == null)
            {
                return Result.Fail(new ExceptionalError($"User with email {email} not found", null)
                    .WithMetadata("ErrorCode", "NOT_FOUND"));
            }
            
            return Result.Ok(DecryptUserModel(model));
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError("Failed to get User by email", ex)
                .WithMetadata("ErrorCode", "FETCH_BY_EMAIL_FAILED"));
        }
    }

    public async Task<Result<int>> Add(User domain)
    {
        try
        {
            var model = EncryptUserModel(domain);
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = 1; // Или получите текущего пользователя
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = 1;

            var sql = @"INSERT INTO ""User"" (
                            email, phone, first_name, last_name, 
                            two_factor_enabled, two_factor_secret,
                            created_at, updated_at, created_by, updated_by
                        ) VALUES (
                            @Email, @Phone, @FirstName, @LastName, 
                            @TwoFactorEnabled, @TwoFactorSecret,
                            @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy
                        ) RETURNING id";

            var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError("Failed to add User", ex)
                .WithMetadata("ErrorCode", "ADD_FAILED"));
        }
    }

    public async Task<Result> Update(User domain)
    {
        try
        {
            var model = EncryptUserModel(domain);
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = 1; // Или получите текущего пользователя

            var sql = @"UPDATE ""User"" SET 
                            email = @Email, 
                            phone = @Phone, 
                            first_name = @FirstName, 
                            last_name = @LastName,
                            two_factor_enabled = @TwoFactorEnabled, 
                            two_factor_secret = @TwoFactorSecret,
                            updated_at = @UpdatedAt, 
                            updated_by = @UpdatedBy
                        WHERE id = @Id";

            var affected = await _dbConnection.ExecuteAsync(sql, model, transaction: _dbTransaction);
            
            if (affected == 0)
            {
                return Result.Fail(new ExceptionalError("User not found", null)
                    .WithMetadata("ErrorCode", "NOT_FOUND"));
            }

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError("Failed to update User", ex)
                .WithMetadata("ErrorCode", "UPDATE_FAILED"));
        }
    }

    public async Task<Result<PaginatedList<User>>> GetPaginated(int pageSize, int pageNumber)
    {
        try
        {
            var sql = @"SELECT id AS ""Id"", 
                               email AS ""Email"", 
                               phone AS ""Phone"", 
                               first_name AS ""FirstName"", 
                               last_name AS ""LastName"",
                               two_factor_enabled AS ""TwoFactorEnabled"",
                               two_factor_secret AS ""TwoFactorSecret""
                        FROM ""User""
                        OFFSET @Offset LIMIT @Limit";
            
            var offset = (pageNumber - 1) * pageSize;
            var models = await _dbConnection.QueryAsync<UserModel>(
                sql, new { Offset = offset, Limit = pageSize }, transaction: _dbTransaction);
            
            var countSql = @"SELECT COUNT(*) FROM ""User""";
            var totalCount = await _dbConnection.ExecuteScalarAsync<int>(countSql, transaction: _dbTransaction);
            
            var users = models.Select(DecryptUserModel).ToList();
            
            return Result.Ok(new PaginatedList<User>(users, totalCount, pageNumber, pageSize));
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError("Failed to get paginated Users", ex)
                .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
        }
    }

    public async Task<Result> Delete(int id)
    {
        try
        {
            var sql = @"DELETE FROM ""User"" WHERE id = @Id";
            var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
            
            if (affected == 0)
            {
                return Result.Fail(new ExceptionalError("User not found", null)
                    .WithMetadata("ErrorCode", "NOT_FOUND"));
            }

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError("Failed to delete User", ex)
                .WithMetadata("ErrorCode", "DELETE_FAILED"));
        }
    }
}