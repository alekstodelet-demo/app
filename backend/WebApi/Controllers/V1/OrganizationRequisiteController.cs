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
    public class OrganizationRequisiteController : BaseController<IOrganizationRequisiteUseCases, OrganizationRequisite, GetOrganizationRequisiteResponse, CreateOrganizationRequisiteRequest, UpdateOrganizationRequisiteRequest>
    {
        private readonly IOrganizationRequisiteUseCases _organizationRequisiteUseCases;
        private readonly ILogger<OrganizationRequisiteController> _logger;
        
        public OrganizationRequisiteController(IOrganizationRequisiteUseCases organizationRequisiteUseCases, ILogger<OrganizationRequisiteController> logger)
            : base(organizationRequisiteUseCases, logger)
        {
            _organizationRequisiteUseCases = organizationRequisiteUseCases ?? throw new ArgumentNullException(nameof(organizationRequisiteUseCases));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        protected override GetOrganizationRequisiteResponse EntityToDtoMapper(OrganizationRequisite entity)
        {
            return entity != null ? GetOrganizationRequisiteResponse.FromDomain(entity) : throw new ArgumentNullException(nameof(entity));
        }
        
        protected override OrganizationRequisite CreateRequestToEntity(CreateOrganizationRequisiteRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        protected override OrganizationRequisite UpdateRequestToEntity(UpdateOrganizationRequisiteRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        [HttpGet("GetByOrganizationId")]
        public async Task<IActionResult> GetByOrganizationId(int organizationId)
        {
            _logger.LogInformation("Getting Organization Requisites for Organization ID: {OrganizationId}", organizationId);
            
            var result = await _organizationRequisiteUseCases.GetByOrganizationId(organizationId);
            if (result.IsSuccess)
            {
                var dtoList = result.Value.Select(GetOrganizationRequisiteResponse.FromDomain).ToList();
                return Ok(dtoList);
            }
            
            return HandleResult(result);
        }
    }
}