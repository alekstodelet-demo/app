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
    public class RepresentativeContactController : BaseController<IRepresentativeContactUseCases, RepresentativeContact, GetRepresentativeContactResponse, CreateRepresentativeContactRequest, UpdateRepresentativeContactRequest>
    {
        private readonly IRepresentativeContactUseCases _representativeContactUseCases;
        private readonly ILogger<RepresentativeContactController> _logger;
        
        public RepresentativeContactController(IRepresentativeContactUseCases representativeContactUseCases, ILogger<RepresentativeContactController> logger)
            : base(representativeContactUseCases, logger)
        {
            _representativeContactUseCases = representativeContactUseCases ?? throw new ArgumentNullException(nameof(representativeContactUseCases));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        protected override GetRepresentativeContactResponse EntityToDtoMapper(RepresentativeContact entity)
        {
            return entity != null ? GetRepresentativeContactResponse.FromDomain(entity) : throw new ArgumentNullException(nameof(entity));
        }
        
        protected override RepresentativeContact CreateRequestToEntity(CreateRepresentativeContactRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        protected override RepresentativeContact UpdateRequestToEntity(UpdateRepresentativeContactRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        [HttpGet("GetByRepresentativeId")]
        public async Task<IActionResult> GetByRepresentativeId(int representativeId)
        {
            _logger.LogInformation("Getting Representative Contacts for Representative ID: {RepresentativeId}", representativeId);
            
            var result = await _representativeContactUseCases.GetByRepresentativeId(representativeId);
            if (result.IsSuccess)
            {
                var dtoList = result.Value.Select(GetRepresentativeContactResponse.FromDomain).ToList();
                return Ok(dtoList);
            }
            
            return HandleResult(result);
        }
    }
}