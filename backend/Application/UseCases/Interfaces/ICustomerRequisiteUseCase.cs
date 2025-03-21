using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public interface ICustomerRequisiteUseCase : IBaseUseCases<Domain.Entities.CustomerRequisite>
    {
        
        Task<List<CustomerRequisite>> GetByOrganizationId(int OrganizationId);
    }
}
