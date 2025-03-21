using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface ICustomerContactRepository : IBaseRepository<CustomerContact>
    {

        Task<Result<List<CustomerContact>>> GetByCustomerId(int customer_id);
        Task<List<CustomerContact>> GetByOrganizationId(int OrganizationId);
    }
}
