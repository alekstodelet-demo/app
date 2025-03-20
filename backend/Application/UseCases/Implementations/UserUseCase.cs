using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class UserUseCases : BaseUseCases<User>, IUserUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<User> Repository => _unitOfWork.UserRepository;

        public UserUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
    }
}
