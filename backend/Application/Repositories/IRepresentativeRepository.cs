using Application.Models;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IRepresentativeRepository : IBaseRepository<Representative>
    {

        Task<List<Representative>> GetByCompanyId(int CompanyId);
        Task<List<Representative>> GetByTypeId(int TypeId);
    }
}
