using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public interface ICustomerContactUseCases : IBaseUseCases<CustomerContact>
    {
        public Task<Result<List<CustomerContact>>> GetByCustomerId(int customer_id);
        public Task<List<CustomerContact>> GetByOrganizationId(int OrganizationId);

    }
}
