using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class OrganizationTypeUseCases : BaseUseCases<OrganizationType>, IOrganizationTypeUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<OrganizationType> Repository => _unitOfWork.OrganizationTypeRepository;

        public OrganizationTypeUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
