using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public interface IRepresentativeUseCase : IBaseUseCases<Domain.Entities.Representative>
    {
        
        Task<List<Representative>> GetByCompanyId(int CompanyId);
        Task<List<Representative>> GetByTypeId(int TypeId);
    }
}
