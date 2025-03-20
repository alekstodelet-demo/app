using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface IRepresentativeContactRepository : IBaseRepository<RepresentativeContact>
    {
        Task<Result<List<RepresentativeContact>>> GetByRepresentativeId(int representativeId);
    }
}
