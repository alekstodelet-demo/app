using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases.Interfaces
{
    public interface IOrganizationContactUseCases : IBaseUseCases<OrganizationContact>
    {
        Task<Result<List<OrganizationContact>>> GetByOrganizationId(int organizationId);
    }
}
