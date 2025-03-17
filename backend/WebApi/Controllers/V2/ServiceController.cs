using Application.UseCases;
using Asp.Versioning;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiController]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ServiceController : BaseController<IServiceUseCases, Service, GetServiceResponse, CreateServiceRequest, UpdateServiceRequest>
    {
        private readonly IServiceUseCases _serviceUseCases;
        private readonly ILogger<ServiceController> _logger;
        
        public ServiceController(IServiceUseCases serviceUseCases, ILogger<ServiceController> logger)
            : base(serviceUseCases, logger)
        {
            _serviceUseCases = serviceUseCases ?? throw new ArgumentNullException(nameof(serviceUseCases));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        protected override GetServiceResponse EntityToDtoMapper(Service entity)
        {
            return entity != null ? GetServiceResponse.FromDomain(entity) : throw new ArgumentNullException(nameof(entity));
        }
        
        protected override Service CreateRequestToEntity(CreateServiceRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        protected override Service UpdateRequestToEntity(UpdateServiceRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveServices()
        {
            _logger.LogInformation("Getting all active services");
            
            var result = await _serviceUseCases.GetAll();
            if (result.IsSuccess)
            {
                var activeServices = result.Value.FirstOrDefault();
                
                return Ok(activeServices);
            }
            
            return HandleResult(result);
        }
    }
}