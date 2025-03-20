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
    public class OrganizationController : BaseController<IOrganizationUseCases, Organization, GetOrganizationResponse, CreateOrganizationRequest, UpdateOrganizationRequest>
    {
        private readonly IOrganizationUseCases _organizationUseCases;
        private readonly ILogger<OrganizationController> _logger;
        
        public OrganizationController(IOrganizationUseCases organizationUseCases, ILogger<OrganizationController> logger)
            : base(organizationUseCases, logger)
        {
            _organizationUseCases = organizationUseCases ?? throw new ArgumentNullException(nameof(organizationUseCases));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        protected override GetOrganizationResponse EntityToDtoMapper(Organization entity)
        {
            return entity != null ? GetOrganizationResponse.FromDomain(entity) : throw new ArgumentNullException(nameof(entity));
        }
        
        protected override Organization CreateRequestToEntity(CreateOrganizationRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        protected override Organization UpdateRequestToEntity(UpdateOrganizationRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
    }
}