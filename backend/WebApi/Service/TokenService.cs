using Application.Exceptions;
using Application.Repositories;
using Application.Services;
using Domain.Entities;
using FluentResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(
            IConfiguration configuration,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Создаем уникальный идентификатор для токена (jti)
            var jti = Guid.NewGuid().ToString();
            var claimsList = claims.ToList();
            claimsList.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claimsList,
                expires: DateTime.UtcNow.AddMinutes(15), // Короткий срок жизни токена - 15 минут
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshToken> GenerateRefreshToken(string userId, string ipAddress, string userAgent, string deviceId)
        {
            using var rng = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rng.GetBytes(randomBytes);
            
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpiryDate = DateTime.UtcNow.AddDays(7), // Refresh token живет 7 дней
                UserId = userId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                DeviceId = deviceId,
                IsRevoked = false,
                IsUsed = false
            };

            // Сохраняем токен в базе данных
            var id = await _refreshTokenRepository.Add(refreshToken);
            refreshToken.Id = id.Value;
            _unitOfWork.Commit();

            return refreshToken;
        }

        public Result<ClaimsPrincipal> ValidateAccessToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero // Убираем стандартную погрешность в 5 минут
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                
                if (validatedToken is not JwtSecurityToken jwtSecurityToken || 
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Result.Fail(new Error("Invalid token").WithMetadata("ErrorCode", "INVALID_TOKEN"));
                }

                return Result.Ok(principal);
            }
            catch (SecurityTokenExpiredException)
            {
                return Result.Fail(new Error("Token expired").WithMetadata("ErrorCode", "TOKEN_EXPIRED"));
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Token validation failed: {ex.Message}").WithMetadata("ErrorCode", "TOKEN_VALIDATION_FAILED"));
            }
        }

        public async Task<Result<RefreshToken>> GetRefreshToken(string token)
        {
            var result = await _refreshTokenRepository.GetByToken(token);
            return result;
        }

        public async Task<bool> IsTokenRevoked(string jti)
        {
            return await _refreshTokenRepository.IsTokenRevoked(jti);
        }

        public async Task<Result> RevokeUserTokens(string userId, string reason)
        {
            var result = await _refreshTokenRepository.RevokeAllUserTokens(userId, reason);
            _unitOfWork.Commit();
            return result;
        }

        public async Task<Result> RevokeRefreshToken(RefreshToken token, string reason, string replacedByToken = null)
        {
            token.IsRevoked = true;
            token.RevokedDate = DateTime.UtcNow;
            token.RevokedReason = reason;
            token.ReplacedByToken = replacedByToken;

            var result = await _refreshTokenRepository.Update(token);
            _unitOfWork.Commit();
            return result;
        }

        public async Task<Result<(string AccessToken, RefreshToken RefreshToken)>> RefreshTokens(
            string refreshToken, string ipAddress, string userAgent, string deviceId)
        {
            var refreshTokenResult = await GetRefreshToken(refreshToken);
            
            if (refreshTokenResult.IsFailed)
            {
                return Result.Fail(refreshTokenResult.Errors);
            }

            var token = refreshTokenResult.Value;

            // Проверяем, что токен не просрочен, не отозван и не использован
            if (token.ExpiryDate < DateTime.UtcNow)
            {
                return Result.Fail(new Error("Token expired").WithMetadata("ErrorCode", "REFRESH_TOKEN_EXPIRED"));
            }

            if (token.IsRevoked)
            {
                return Result.Fail(new Error("Token revoked").WithMetadata("ErrorCode", "REFRESH_TOKEN_REVOKED"));
            }

            if (token.IsUsed)
            {
                return Result.Fail(new Error("Token used").WithMetadata("ErrorCode", "REFRESH_TOKEN_USED"));
            }

            // Проверяем соответствие IP и User-Agent для защиты от кражи токена
            // Если отличается deviceId, это может быть признаком кражи токена
            if (token.DeviceId != deviceId)
            {
                await RevokeRefreshToken(token, "Suspected token theft: different device ID");
                await RevokeUserTokens(token.UserId, "Suspected token theft: all user tokens revoked");
                return Result.Fail(new Error("Token theft detected").WithMetadata("ErrorCode", "TOKEN_THEFT_DETECTED"));
            }

            // Опционально можно проверять IP и User-Agent, но это может вызвать проблемы 
            // при смене IP или обновлении браузера
            
            // Помечаем текущий токен как использованный
            token.IsUsed = true;
            await _refreshTokenRepository.Update(token);

            // Генерируем claims для нового токена
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, token.UserId),
                new Claim("ip_address", ipAddress),
                new Claim("user_agent", userAgent.Substring(0, Math.Min(userAgent.Length, 255))),
                new Claim("device_id", deviceId)
                // Можно добавить и другие claims, например, роли пользователя
            };

            // Генерируем новую пару токенов
            var newAccessToken = GenerateAccessToken(claims);
            var newRefreshToken = await GenerateRefreshToken(token.UserId, ipAddress, userAgent, deviceId);

            // Обновляем старый токен, указывая, каким токеном он был заменен
            await RevokeRefreshToken(token, "Replaced by new token", newRefreshToken.Token);

            _unitOfWork.Commit();

            return Result.Ok((newAccessToken, newRefreshToken));
        }
    }
}