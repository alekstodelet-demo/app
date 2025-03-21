using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        
        Task<List<Customer>> GetByOrganizationTypeId(int OrganizationTypeId);
    }
}
