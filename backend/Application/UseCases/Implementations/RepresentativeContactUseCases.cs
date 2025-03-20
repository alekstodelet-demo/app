using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class RepresentativeContactUseCases : BaseUseCases<RepresentativeContact>, IRepresentativeContactUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<RepresentativeContact> Repository => _unitOfWork.RepresentativeContactRepository;

        public RepresentativeContactUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public Task<Result<List<RepresentativeContact>>> GetByRepresentativeId(int representativeId)
        {
            return _unitOfWork.RepresentativeContactRepository.GetByRepresentativeId(representativeId);
        }
    }
}
