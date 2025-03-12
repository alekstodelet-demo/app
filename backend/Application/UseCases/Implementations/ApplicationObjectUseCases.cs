using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class ApplicationObjectUseCases : BaseUseCases<ApplicationObject>, IApplicationObjectUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<ApplicationObject> Repository => _unitOfWork.ApplicationObjectRepository;

        public ApplicationObjectUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
    }
}
