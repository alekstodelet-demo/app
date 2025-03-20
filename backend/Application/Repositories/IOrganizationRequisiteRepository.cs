using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface IOrganizationRequisiteRepository : IBaseRepository<OrganizationRequisite>
    {
        Task<Result<List<OrganizationRequisite>>> GetByOrganizationId(int organizationId);
    }
}
