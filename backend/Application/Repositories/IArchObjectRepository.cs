using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface IArchObjectRepository : IBaseRepository<ArchObject>
    {
        Task<Result<List<ArchObject>>> GetBySearch(string text);
        Task<Result<List<ArchObject>>> GetByApplicationId(int applicationId);
    }
}
