using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class ArchObjectUseCases : BaseUseCases<ArchObject>, IArchObjectUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<ArchObject> Repository => _unitOfWork.ArchObjectRepository;

        public ArchObjectUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
    }
}
