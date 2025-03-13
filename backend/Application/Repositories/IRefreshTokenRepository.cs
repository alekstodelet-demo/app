using Application.Models;
using Domain.Entities;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        Task<Result<RefreshToken>> GetByToken(string token);
        Task<bool> IsTokenRevoked(string jti);
        Task<Result<List<RefreshToken>>> GetUserTokens(string userId);
        Task<Result> RevokeAllUserTokens(string userId, string reason);
    }
}