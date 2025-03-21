using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface IRepresentativeContactRepository : IBaseRepository<RepresentativeContact>
    {
        
        Task<List<RepresentativeContact>> GetByRepresentativeId(int RepresentativeId);
    }
}
