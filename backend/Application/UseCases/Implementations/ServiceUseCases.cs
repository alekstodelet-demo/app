using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public class ServiceUseCases : IServiceUseCases
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceUseCases(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<List<Service>>> GetAll()
        {
            return _unitOfWork.ServiceRepository.GetAll();
        }

        public Task<Result<Service>> GetOneByID(int id)
        {
            return _unitOfWork.ServiceRepository.GetOneByID(id);
        }

        public async Task<Result<Service>> Create(Service domain)
        {
            var result = await _unitOfWork.ServiceRepository.Add(domain);
            domain.Id = result.Value;
            _unitOfWork.Commit();
            return Result.Ok(domain);
        }

        public async Task<Result<Service>> Update(Service domain)
        {
            await _unitOfWork.ServiceRepository.Update(domain);
            _unitOfWork.Commit();
            return domain;
        }

        public Task<Result<PaginatedList<Service>>> GetPaginated(int pageSize, int pageNumber)
        {
            if (pageSize < 1) pageSize = 1;
            if (pageNumber < 1) pageNumber = 1;
            return _unitOfWork.ServiceRepository.GetPaginated(pageSize, pageNumber);
        }

        public async Task<Result> Delete(int id)
        {
            await _unitOfWork.ServiceRepository.Delete(id);
            _unitOfWork.Commit();
            return Result.Ok();
        }
    }
}
