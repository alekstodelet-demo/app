using Application.Models;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entities;

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


        public Task<List<RepresentativeContact>> GetByRepresentativeId(int RepresentativeId)
        {
            return _unitOfWork.RepresentativeContactRepository.GetByRepresentativeId(RepresentativeId);
        }

    }
}
