using Application.Models;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class RepresentativeTypeUseCases : BaseUseCases<RepresentativeType>, IRepresentativeTypeUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<RepresentativeType> Repository => _unitOfWork.RepresentativeTypeRepository;

        public RepresentativeTypeUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


    }
}
