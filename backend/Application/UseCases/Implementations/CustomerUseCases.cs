using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class CustomerUseCases : BaseUseCases<Customer>, ICustomerUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<Customer> Repository => _unitOfWork.CustomerRepository;

        public CustomerUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public Task<List<Customer>> GetByOrganizationTypeId(int OrganizationTypeId)
        {
            return _unitOfWork.CustomerRepository.GetByOrganizationTypeId(OrganizationTypeId);
        }
        
    }
}
