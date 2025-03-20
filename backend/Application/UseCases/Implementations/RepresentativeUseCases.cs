using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class RepresentativeUseCases : BaseUseCases<Representative>, IRepresentativeUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<Representative> Repository => _unitOfWork.RepresentativeRepository;

        public RepresentativeUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public Task<Result<List<Representative>>> GetByOrganizationId(int organizationId)
        {
            return _unitOfWork.RepresentativeRepository.GetByOrganizationId(organizationId);
        }
    }
}
