using Application.Models;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IServiceRepository : BaseRepository
    {
        Task<List<Service>> GetAll();
        Task<Service> GetOneByID(int id);
        Task<PaginatedList<Service>> GetPaginated(int pageSize, int pageNumber);
        Task<int> Add(Service domain);
        Task Update(Service domain);
        Task Delete(int id);
    }
}
