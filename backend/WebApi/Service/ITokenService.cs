using Domain.Entities;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        
        Task<RefreshToken> GenerateRefreshToken(string userId, string ipAddress, string userAgent, string deviceId);
        
        Result<ClaimsPrincipal> ValidateAccessToken(string token);
        
        Task<Result<RefreshToken>> GetRefreshToken(string token);
        
        Task<bool> IsTokenRevoked(string jti);
        
        Task<Result> RevokeUserTokens(string userId, string reason);
        
        Task<Result> RevokeRefreshToken(RefreshToken token, string reason, string replacedByToken = null);
        
        Task<Result<(string AccessToken, RefreshToken RefreshToken)>> RefreshTokens(
            string refreshToken, string ipAddress, string userAgent, string deviceId);
    }
}