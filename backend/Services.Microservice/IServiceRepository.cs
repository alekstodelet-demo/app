using System.Data;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface IServiceRepository
    {
        Task<Result<List<Service>>> GetAll();
        Task<Result<Service>> GetOneByID(int id);
        Task<Result<int>> Add(Service entity);
        Task<Result> Update(Service entity);
        Task<Result> Delete(int id);
        void SetTransaction(IDbTransaction dbTransaction);
    }
}
