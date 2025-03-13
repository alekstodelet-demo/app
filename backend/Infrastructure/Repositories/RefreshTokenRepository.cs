using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.Repositories;
using Dapper;
using Domain.Entities;
using FluentResults;
using Infrastructure.FillLogData;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;

        public RefreshTokenRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<int>> Add(RefreshToken entity)
        {

            entity.CreatedAt = DateTime.Now;
            entity.CreatedBy = 1;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = 1;
            try
            {
                var sql = @"INSERT INTO refresh_token(
                                token, 
                                expiry_date, 
                                is_revoked, 
                                is_used, 
                                user_id, 
                                ip_address, 
                                user_agent, 
                                device_id, 
                                revoked_date, 
                                replaced_by_token, 
                                revoked_reason, 
                                created_at, 
                                updated_at, 
                                created_by, 
                                updated_by)
                            VALUES (
                                @Token, 
                                @ExpiryDate, 
                                @IsRevoked, 
                                @IsUsed, 
                                @UserId, 
                                @IpAddress, 
                                @UserAgent, 
                                @DeviceId, 
                                @RevokedDate, 
                                @ReplacedByToken, 
                                @RevokedReason, 
                                @CreatedAt, 
                                @UpdatedAt, 
                                @CreatedBy, 
                                @UpdatedBy)
                            RETURNING id";

                var id = await _dbConnection.ExecuteScalarAsync<int>(sql, entity, transaction: _dbTransaction);
                return Result.Ok(id);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Failed to add refresh token: {ex.Message}")
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = "DELETE FROM refresh_token WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
                
                if (affected == 0)
                {
                    return Result.Fail(new Error($"Refresh token with ID {id} not found.")
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Failed to delete refresh token: {ex.Message}")
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        public async Task<Result<List<RefreshToken>>> GetAll()
        {
            try
            {
                var sql = @"SELECT 
                                id AS ""Id"",
                                token AS ""Token"",
                                expiry_date AS ""ExpiryDate"",
                                is_revoked AS ""IsRevoked"",
                                is_used AS ""IsUsed"",
                                user_id AS ""UserId"",
                                ip_address AS ""IpAddress"",
                                user_agent AS ""UserAgent"",
                                device_id AS ""DeviceId"",
                                revoked_date AS ""RevokedDate"",
                                replaced_by_token AS ""ReplacedByToken"",
                                revoked_reason AS ""RevokedReason"",
                                created_at,
                                updated_at,
                                created_by,
                                updated_by
                            FROM refresh_token";
                
                var result = await _dbConnection.QueryAsync<RefreshToken>(sql, transaction: _dbTransaction);
                return Result.Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Failed to get refresh tokens: {ex.Message}")
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<RefreshToken>> GetByToken(string token)
        {
            try
            {
                var sql = @"SELECT 
                                id AS ""Id"",
                                token AS ""Token"",
                                expiry_date AS ""ExpiryDate"",
                                is_revoked AS ""IsRevoked"",
                                is_used AS ""IsUsed"",
                                user_id AS ""UserId"",
                                ip_address AS ""IpAddress"",
                                user_agent AS ""UserAgent"",
                                device_id AS ""DeviceId"",
                                revoked_date AS ""RevokedDate"",
                                replaced_by_token AS ""ReplacedByToken"",
                                revoked_reason AS ""RevokedReason"",
                                created_at,
                                updated_at,
                                created_by,
                                updated_by
                            FROM refresh_token
                            WHERE token = @Token";
                
                var result = await _dbConnection.QuerySingleOrDefaultAsync<RefreshToken>(
                    sql, new { Token = token }, transaction: _dbTransaction);
                
                if (result == null)
                {
                    return Result.Fail(new Error("Refresh token not found")
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Failed to get refresh token: {ex.Message}")
                    .WithMetadata("ErrorCode", "FETCH_BY_TOKEN_FAILED"));
            }
        }

        public async Task<Result<RefreshToken>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT 
                                id AS ""Id"",
                                token AS ""Token"",
                                expiry_date AS ""ExpiryDate"",
                                is_revoked AS ""IsRevoked"",
                                is_used AS ""IsUsed"",
                                user_id AS ""UserId"",
                                ip_address AS ""IpAddress"",
                                user_agent AS ""UserAgent"",
                                device_id AS ""DeviceId"",
                                revoked_date AS ""RevokedDate"",
                                replaced_by_token AS ""ReplacedByToken"",
                                revoked_reason AS ""RevokedReason"",
                                created_at,
                                updated_at,
                                created_by,
                                updated_by
                            FROM refresh_token
                            WHERE id = @Id";
                
                var result = await _dbConnection.QuerySingleOrDefaultAsync<RefreshToken>(
                    sql, new { Id = id }, transaction: _dbTransaction);
                
                if (result == null)
                {
                    return Result.Fail(new Error($"Refresh token with ID {id} not found.")
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Failed to get refresh token: {ex.Message}")
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<RefreshToken>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT 
                                id AS ""Id"",
                                token AS ""Token"",
                                expiry_date AS ""ExpiryDate"",
                                is_revoked AS ""IsRevoked"",
                                is_used AS ""IsUsed"",
                                user_id AS ""UserId"",
                                ip_address AS ""IpAddress"",
                                user_agent AS ""UserAgent"",
                                device_id AS ""DeviceId"",
                                revoked_date AS ""RevokedDate"",
                                replaced_by_token AS ""ReplacedByToken"",
                                revoked_reason AS ""RevokedReason"",
                                created_at,
                                updated_at,
                                created_by,
                                updated_by
                            FROM refresh_token
                            ORDER BY id
                            OFFSET @Offset LIMIT @Limit";
                
                var offset = (pageNumber - 1) * pageSize;
                var result = await _dbConnection.QueryAsync<RefreshToken>(
                    sql, new { Offset = offset, Limit = pageSize }, transaction: _dbTransaction);
                
                var countSql = "SELECT COUNT(*) FROM refresh_token";
                var totalCount = await _dbConnection.ExecuteScalarAsync<int>(countSql, transaction: _dbTransaction);
                
                var items = result.ToList();
                return Result.Ok(new PaginatedList<RefreshToken>(items, totalCount, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Failed to get paginated refresh tokens: {ex.Message}")
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result<List<RefreshToken>>> GetUserTokens(string userId)
        {
            try
            {
                var sql = @"SELECT 
                                id AS ""Id"",
                                token AS ""Token"",
                                expiry_date AS ""ExpiryDate"",
                                is_revoked AS ""IsRevoked"",
                                is_used AS ""IsUsed"",
                                user_id AS ""UserId"",
                                ip_address AS ""IpAddress"",
                                user_agent AS ""UserAgent"",
                                device_id AS ""DeviceId"",
                                revoked_date AS ""RevokedDate"",
                                replaced_by_token AS ""ReplacedByToken"",
                                revoked_reason AS ""RevokedReason"",
                                created_at,
                                updated_at,
                                created_by,
                                updated_by
                            FROM refresh_token
                            WHERE user_id = @UserId";
                
                var result = await _dbConnection.QueryAsync<RefreshToken>(
                    sql, new { UserId = userId }, transaction: _dbTransaction);
                
                return Result.Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Failed to get user tokens: {ex.Message}")
                    .WithMetadata("ErrorCode", "FETCH_USER_TOKENS_FAILED"));
            }
        }

        public async Task<bool> IsTokenRevoked(string jti)
        {
            try
            {
                var sql = "SELECT COUNT(*) FROM revoked_tokens WHERE jti = @Jti";
                var count = await _dbConnection.ExecuteScalarAsync<int>(
                    sql, new { Jti = jti }, transaction: _dbTransaction);
                
                return count > 0;
            }
            catch
            {
                // В случае ошибки, считаем токен отозванным для безопасности
                return true;
            }
        }

        public async Task<Result> RevokeAllUserTokens(string userId, string reason)
        {
            try
            {
                var currentDateTime = DateTime.UtcNow;
                var sql = @"UPDATE refresh_token
                            SET is_revoked = true,
                                revoked_date = @RevokedDate,
                                revoked_reason = @RevokedReason,
                                updated_at = @UpdatedAt,
                                updated_by = @UpdatedBy
                            WHERE user_id = @UserId
                                  AND is_revoked = false";
                
                
                var affected = await _dbConnection.ExecuteAsync(
                    sql, 
                    new { 
                        UserId = userId, 
                        RevokedDate = currentDateTime, 
                        RevokedReason = reason,
                        UpdatedAt = currentDateTime,
                        UpdatedBy = 1
                    }, 
                    transaction: _dbTransaction);
                
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Failed to revoke all user tokens: {ex.Message}")
                    .WithMetadata("ErrorCode", "REVOKE_ALL_FAILED"));
            }
        }

        public async Task<Result> Update(RefreshToken entity)
        {
            try
            {
                var sql = @"UPDATE refresh_token
                            SET token = @Token,
                                expiry_date = @ExpiryDate,
                                is_revoked = @IsRevoked,
                                is_used = @IsUsed,
                                user_id = @UserId,
                                ip_address = @IpAddress,
                                user_agent = @UserAgent,
                                device_id = @DeviceId,
                                revoked_date = @RevokedDate,
                                replaced_by_token = @ReplacedByToken,
                                revoked_reason = @RevokedReason,
                                updated_at = @updated_at,
                                updated_by = @updated_by
                            WHERE id = @Id";
                
                var affected = await _dbConnection.ExecuteAsync(sql, entity, transaction: _dbTransaction);
                
                if (affected == 0)
                {
                    return Result.Fail(new Error($"Refresh token with ID {entity.Id} not found.")
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Failed to update refresh token: {ex.Message}")
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }
    }
}