using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface IOrganizationContactRepository : IBaseRepository<OrganizationContact>
    {
        Task<Result<List<OrganizationContact>>> GetByOrganizationId(int organizationId);
    }
}
