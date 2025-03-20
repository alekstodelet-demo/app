using Application.UseCases;
using Asp.Versioning;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrganizationTypeController : BaseController<IOrganizationTypeUseCases, OrganizationType, GetOrganizationTypeResponse, CreateOrganizationTypeRequest, UpdateOrganizationTypeRequest>
    {
        private readonly IOrganizationTypeUseCases _organizationTypeUseCases;
        private readonly ILogger<OrganizationTypeController> _logger;
        
        public OrganizationTypeController(IOrganizationTypeUseCases organizationTypeUseCases, ILogger<OrganizationTypeController> logger)
            : base(organizationTypeUseCases, logger)
        {
            _organizationTypeUseCases = organizationTypeUseCases ?? throw new ArgumentNullException(nameof(organizationTypeUseCases));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        protected override GetOrganizationTypeResponse EntityToDtoMapper(OrganizationType entity)
        {
            return entity != null ? GetOrganizationTypeResponse.FromDomain(entity) : throw new ArgumentNullException(nameof(entity));
        }
        
        protected override OrganizationType CreateRequestToEntity(CreateOrganizationTypeRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        protected override OrganizationType UpdateRequestToEntity(UpdateOrganizationTypeRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
    }
}