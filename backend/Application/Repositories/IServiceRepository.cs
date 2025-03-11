using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface IServiceRepository : BaseRepository
    {
        Task<Result<List<Service>>> GetAll();
        Task<Result<Service>> GetOneByID(int id);
        Task<Result<PaginatedList<Service>>> GetPaginated(int pageSize, int pageNumber);
        Task<Result<int>> Add(Service domain);
        Task<Result> Update(Service domain);
        Task<Result> Delete(int id);
    }
}
