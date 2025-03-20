using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public interface IOrganizationRequisiteUseCases : IBaseUseCases<OrganizationRequisite>
    {
        Task<Result<List<OrganizationRequisite>>> GetByOrganizationId(int organizationId);
    }
}
