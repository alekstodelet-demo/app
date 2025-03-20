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
    public class RepresentativeController : BaseController<IRepresentativeUseCases, Representative, GetRepresentativeResponse, CreateRepresentativeRequest, UpdateRepresentativeRequest>
    {
        private readonly IRepresentativeUseCases _representativeUseCases;
        private readonly ILogger<RepresentativeController> _logger;
        
        public RepresentativeController(IRepresentativeUseCases representativeUseCases, ILogger<RepresentativeController> logger)
            : base(representativeUseCases, logger)
        {
            _representativeUseCases = representativeUseCases ?? throw new ArgumentNullException(nameof(representativeUseCases));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        protected override GetRepresentativeResponse EntityToDtoMapper(Representative entity)
        {
            return entity != null ? GetRepresentativeResponse.FromDomain(entity) : throw new ArgumentNullException(nameof(entity));
        }
        
        protected override Representative CreateRequestToEntity(CreateRepresentativeRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        protected override Representative UpdateRequestToEntity(UpdateRepresentativeRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        [HttpGet("GetByOrganizationId")]
        public async Task<IActionResult> GetByOrganizationId(int organizationId)
        {
            _logger.LogInformation("Getting Representatives for Organization ID: {OrganizationId}", organizationId);
            
            var result = await _representativeUseCases.GetByOrganizationId(organizationId);
            if (result.IsSuccess)
            {
                var dtoList = result.Value.Select(GetRepresentativeResponse.FromDomain).ToList();
                return Ok(dtoList);
            }
            
            return HandleResult(result);
        }
    }
}