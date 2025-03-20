using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases.Interfaces
{
    public interface IRepresentativeContactUseCases : IBaseUseCases<RepresentativeContact>
    {
        Task<Result<List<RepresentativeContact>>> GetByRepresentativeId(int representativeId);
    }
}
