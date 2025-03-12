using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface IApplicationObjectRepository : IBaseRepository<ApplicationObject>
    {
        Task<Result<List<ApplicationObject>>> GetByApplicationId(int applicationId);
        Task<Result<List<ApplicationObject>>> GetByArchObjectId(int archObjectId);
    }
}
