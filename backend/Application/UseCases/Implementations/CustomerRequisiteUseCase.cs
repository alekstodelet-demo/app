using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class CustomerRequisiteUseCases : BaseUseCases<CustomerRequisite>, ICustomerRequisiteUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<CustomerRequisite> Repository => _unitOfWork.CustomerRequisiteRepository;

        public CustomerRequisiteUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public Task<List<CustomerRequisite>> GetByOrganizationId(int OrganizationId)
        {
            return _unitOfWork.CustomerRequisiteRepository.GetByOrganizationId(OrganizationId);
        }
        
    }
}
