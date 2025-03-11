using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public interface IBaseUseCases<T>
    {
        Task<Result<List<T>>> GetAll();
        Task<Result<T>> GetOneByID(int id);
        Task<Result<PaginatedList<T>>> GetPaginated(int pageSize, int pageNumber);
        Task<Result<T>> Create(T domain);
        Task<Result<T>> Update(T domain);
        Task<Result> Delete(int id);
    }
}
