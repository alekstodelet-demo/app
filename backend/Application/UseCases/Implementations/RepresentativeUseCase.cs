using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class RepresentativeUseCases : BaseUseCases<Representative>, IRepresentativeUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        protected override IBaseRepository<Representative> Repository => _unitOfWork.RepresentativeRepository;

        public RepresentativeUseCases(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override Task<Result<Representative>> Create(Representative domain)
        {
            mockFetchTundukData(domain);
            return base.Create(domain);
        }


        private void mockFetchTundukData(Representative representative)
        {
            representative.LastName = "Иванов";
            representative.FirstName = "Иван";
            representative.SecondName = "Иванович";
        }

        public Task<List<Representative>> GetByCompanyId(int CompanyId)
        {
            return _unitOfWork.RepresentativeRepository.GetByCompanyId(CompanyId);
        }
        
        public Task<List<Representative>> GetByTypeId(int TypeId)
        {
            return _unitOfWork.RepresentativeRepository.GetByTypeId(TypeId);
        }
        
    }
}
