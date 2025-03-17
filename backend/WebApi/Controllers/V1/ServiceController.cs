using Application.UseCases;
using Asp.Versioning;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiController]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ServiceController : BaseController<IServiceUseCases, Service, GetServiceResponse, CreateServiceRequest,
        UpdateServiceRequest>
    {
        private readonly IServiceUseCases _serviceUseCases;

        public ServiceController(IServiceUseCases serviceUseCases,
            ILogger<BaseController<IServiceUseCases, Service, GetServiceResponse, CreateServiceRequest,
                UpdateServiceRequest>> logger)
            : base(serviceUseCases, logger)
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