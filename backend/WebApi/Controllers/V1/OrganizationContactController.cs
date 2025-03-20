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
    public class OrganizationContactController : BaseController<IOrganizationContactUseCases, OrganizationContact, GetOrganizationContactResponse, CreateOrganizationContactRequest, UpdateOrganizationContactRequest>
    {
        private readonly IOrganizationContactUseCases _organizationContactUseCases;
        private readonly ILogger<OrganizationContactController> _logger;
        
        public OrganizationContactController(IOrganizationContactUseCases organizationContactUseCases, ILogger<OrganizationContactController> logger)
            : base(organizationContactUseCases, logger)
        {
            _organizationContactUseCases = organizationContactUseCases ?? throw new ArgumentNullException(nameof(organizationContactUseCases));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        protected override GetOrganizationContactResponse EntityToDtoMapper(OrganizationContact entity)
        {
            return entity != null ? GetOrganizationContactResponse.FromDomain(entity) : throw new ArgumentNullException(nameof(entity));
        }
        
        protected override OrganizationContact CreateRequestToEntity(CreateOrganizationContactRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        protected override OrganizationContact UpdateRequestToEntity(UpdateOrganizationContactRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        [HttpGet("GetByOrganizationId")]
        public async Task<IActionResult> GetByOrganizationId(int organizationId)
        {
            _logger.LogInformation("Getting Organization Contacts for Organization ID: {OrganizationId}", organizationId);
            
            var result = await _organizationContactUseCases.GetByOrganizationId(organizationId);
            if (result.IsSuccess)
            {
                var dtoList = result.Value.Select(GetOrganizationContactResponse.FromDomain).ToList();
                return Ok(dtoList);
            }
            
            return HandleResult(result);
        }
    }
}