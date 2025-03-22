using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class RepresentativeContactUseCases : BaseUseCases<RepresentativeContact>, IRepresentativeContactUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<RepresentativeContact> Repository => _unitOfWork.RepresentativeContactRepository;

        public RepresentativeContactUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task<Result<RepresentativeContact>> Create(RepresentativeContact domain)
        {
            var result = await base.Create(domain);
            var representative = await _unitOfWork.RepresentativeRepository.GetOneByID(domain.RepresentativeId);
            representative.Value.HasAccess = true;
            await _unitOfWork.RepresentativeRepository.Update(representative.Value);
            _unitOfWork.Commit();

            return result;
        }

        public Task<List<RepresentativeContact>> GetByRepresentativeId(int RepresentativeId)
        {
            return _unitOfWork.RepresentativeContactRepository.GetByRepresentativeId(RepresentativeId);
        }
        
    }
}
