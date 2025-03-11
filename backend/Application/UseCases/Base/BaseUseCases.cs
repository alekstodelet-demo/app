using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public abstract class BaseUseCases<TEntity> : IBaseUseCases<TEntity> where TEntity : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected abstract IBaseRepository<TEntity> Repository { get; }

        protected BaseUseCases(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public virtual Task<Result<List<TEntity>>> GetAll() => Repository.GetAll();
        
        public virtual Task<Result<TEntity>> GetOneByID(int id) => Repository.GetOneByID(id);

        public virtual async Task<Result<TEntity>> Create(TEntity domain)
        {
            var result = await Repository.Add(domain);
            if (result.IsSuccess)
            {
                _unitOfWork.Commit();
                return Result.Ok(domain);
            }
            return Result.Fail(result.Errors);
        }

        public virtual async Task<Result<TEntity>> Update(TEntity domain)
        {
            await Repository.Update(domain);
            _unitOfWork.Commit();
            return Result.Ok(domain);
        }

        public virtual Task<Result<PaginatedList<TEntity>>> GetPaginated(int pageSize, int pageNumber)
        {
            if (pageSize < 1) pageSize = 1;
            if (pageNumber < 1) pageNumber = 1;
            return Repository.GetPaginated(pageSize, pageNumber);
        }

        public virtual async Task<Result> Delete(int id)
        {
            await Repository.Delete(id);
            _unitOfWork.Commit();
            return Result.Ok();
        }
    }
}