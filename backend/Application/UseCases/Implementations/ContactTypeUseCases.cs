using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class ContactTypeUseCases : BaseUseCases<ContactType>, IContactTypeUseCases
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<ContactType> Repository => _unitOfWork.ContactTypeRepository;

        public ContactTypeUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
    }
}
