using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface ICustomerContactRepository : IBaseRepository<CustomerContact>
    {
        public Task<Result<List<CustomerContact>>> GetByCustomerId(int customerId);
    }
}
