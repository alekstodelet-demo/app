using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class OrganizationContactUseCases : BaseUseCases<OrganizationContact>, IOrganizationContactUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<OrganizationContact> Repository => _unitOfWork.OrganizationContactRepository;

        public OrganizationContactUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public Task<Result<List<OrganizationContact>>> GetByOrganizationId(int organizationId)
        {
            return _unitOfWork.OrganizationContactRepository.GetByOrganizationId(organizationId);
        }
    }
}
