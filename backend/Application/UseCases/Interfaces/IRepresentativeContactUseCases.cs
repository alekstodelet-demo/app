using Application.Models;
using Domain.Entities;

namespace Application.UseCases
{
    public interface IRepresentativeContactUseCase : IBaseUseCases<Domain.Entities.RepresentativeContact>
    {

        Task<List<RepresentativeContact>> GetByRepresentativeId(int RepresentativeId);
    }
}
