using Application.Models;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public interface IRepresentativeContactUseCases : IBaseUseCases<RepresentativeContact>
    {
        Task<Result<List<RepresentativeContact>>> GetByRepresentativeId(int representativeId);
    }
}