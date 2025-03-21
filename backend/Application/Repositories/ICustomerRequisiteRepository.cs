using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface ICustomerRequisiteRepository : IBaseRepository<CustomerRequisite>
    {
        
        Task<List<CustomerRequisite>> GetByOrganizationId(int OrganizationId);
    }
}
