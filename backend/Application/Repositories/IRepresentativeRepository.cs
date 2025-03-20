using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface IRepresentativeRepository : IBaseRepository<Representative>
    {
        Task<Result<List<Representative>>> GetByOrganizationId(int organizationId);
    }
}
