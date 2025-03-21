using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public interface ICustomerUseCases : IBaseUseCases<Customer>
    {
       public Task<List<Customer>> GetByOrganizationTypeId(int OrganizationTypeId);

    }
}
