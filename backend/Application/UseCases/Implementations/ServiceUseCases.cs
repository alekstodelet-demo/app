using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class ServiceUseCases : BaseUseCases<Service>, IServiceUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<Service> Repository => _unitOfWork.ServiceRepository;

        public ServiceUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
    }
}
