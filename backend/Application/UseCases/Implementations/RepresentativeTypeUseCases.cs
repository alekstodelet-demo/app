using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class RepresentativeTypeUseCases : BaseUseCases<RepresentativeType>, IRepresentativeTypeUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<RepresentativeType> Repository => _unitOfWork.RepresentativeTypeRepository;

        public RepresentativeTypeUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
