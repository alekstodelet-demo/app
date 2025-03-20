using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class OrganizationUseCases : BaseUseCases<Organization>, IOrganizationUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<Organization> Repository => _unitOfWork.OrganizationRepository;

        public OrganizationUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
