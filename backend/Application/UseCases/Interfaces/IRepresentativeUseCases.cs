using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases.Interfaces
{
    public interface IRepresentativeUseCases : IBaseUseCases<Representative>
    {
        Task<Result<List<Representative>>> GetByOrganizationId(int organizationId);
    }
}
