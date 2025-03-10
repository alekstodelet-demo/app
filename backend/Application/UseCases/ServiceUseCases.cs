using Application.Models;
using Application.Repositories;
using Domain.Entities;

namespace Application.UseCases
{
    public class ServiceUseCases
    {
        private readonly IUnitOfWork unitOfWork;

        public ServiceUseCases(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task<List<Service>> GetAll()
        {
            return unitOfWork.ServiceRepository.GetAll();
        }

        public Task<Service> GetOneByID(int id)
        {
            return unitOfWork.ServiceRepository.GetOneByID(id);
        }

        public async Task<Service> Create(Service domain)
        {
            var result = await unitOfWork.ServiceRepository.Add(domain);
            domain.id = result;
            unitOfWork.Commit();
            return domain;
        }

        public async Task<Service> Update(Service domain)
        {
            await unitOfWork.ServiceRepository.Update(domain);
            unitOfWork.Commit();
            return domain;
        }

        public Task<PaginatedList<Service>> GetPagniated(int pageSize, int pageNumber)
        {
            if (pageSize < 1) pageSize = 1;
            if (pageNumber < 1) pageNumber = 1;
            return unitOfWork.ServiceRepository.GetPaginated(pageSize, pageNumber);
        }

        public async Task Delete(int id)
        {
            await unitOfWork.ServiceRepository.Delete(id);
            unitOfWork.Commit();
        }
    }
}
