using System.Data;
using Application.Models;
using FluentResults;

namespace Application.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<Result<List<T>>> GetAll();
        Task<Result<T>> GetOneByID(int id);
        Task<Result<int>> Add(T entity);
        Task<Result> Update(T entity);
        Task<Result<PaginatedList<T>>> GetPaginated(int pageSize, int pageNumber);
        Task<Result> Delete(int id);
        void SetTransaction(IDbTransaction dbTransaction);
    }
}
