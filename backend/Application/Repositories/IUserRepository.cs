using Domain.Entities;
using FluentResults;

namespace Application.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<Result<User>> GetByEmail(string email);
}