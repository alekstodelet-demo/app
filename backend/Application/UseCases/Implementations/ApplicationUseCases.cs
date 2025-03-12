using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class ApplicationUseCases : BaseUseCases<Domain.Entities.Application>, IApplicationUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<Domain.Entities.Application> Repository => _unitOfWork.ApplicationRepository;

        public ApplicationUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
    }
}
