using FluentResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using Application.UseCases;
using Domain.Entities;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : BaseController<IServiceUseCases, Service, GetServiceResponse, CreateServiceRequest, UpdateServiceRequest>
    {
        private readonly IServiceUseCases _serviceUseCases;

        public ServiceController(IServiceUseCases serviceUseCases) : base(serviceUseCases)
        {
            _serviceUseCases = serviceUseCases;
        }
        
        protected override GetServiceResponse EntityToDtoMapper(Service entity)
        {
            return GetServiceResponse.FromDomain(entity);
        }
        
        protected override Service CreateRequestToEntity(CreateServiceRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override Service UpdateRequestToEntity(UpdateServiceRequest requestDto)
        {
            return requestDto.ToDomain();
        }
    }
}