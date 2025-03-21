using Application.Models;
using Domain.Entities;


namespace Application.Repositories
{
    public interface IRepresentativeTypeRepository : IBaseRepository<RepresentativeType>
    {
        Task<List<RepresentativeType>> GetAll();
    }
}
