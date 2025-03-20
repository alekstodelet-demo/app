using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class OrganizationRequisiteUseCases : BaseUseCases<OrganizationRequisite>, IOrganizationRequisiteUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<OrganizationRequisite> Repository => _unitOfWork.OrganizationRequisiteRepository;

        public OrganizationRequisiteUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<List<OrganizationRequisite>>> GetByOrganizationId(int organizationId)
        {
            return _unitOfWork.OrganizationRequisiteRepository.GetByOrganizationId(organizationId);
        }
    }
}