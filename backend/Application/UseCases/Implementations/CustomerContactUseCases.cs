using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class CustomerContactUseCases : BaseUseCases<CustomerContact>, ICustomerContactUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<CustomerContact> Repository => _unitOfWork.CustomerContactRepository;

        public CustomerContactUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Result<List<CustomerContact>>> GetByCustomerId(int customer_id)
        {
            return _unitOfWork.CustomerContactRepository.GetByCustomerId(customer_id);
        }

    }
}
