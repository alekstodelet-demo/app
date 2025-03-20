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
    public class RepresentativeTypeController : BaseController<IRepresentativeTypeUseCases, RepresentativeType, GetRepresentativeTypeResponse, CreateRepresentativeTypeRequest, UpdateRepresentativeTypeRequest>
    {
        private readonly IRepresentativeTypeUseCases _representativeTypeUseCases;
        private readonly ILogger<RepresentativeTypeController> _logger;
        
        public RepresentativeTypeController(IRepresentativeTypeUseCases representativeTypeUseCases, ILogger<RepresentativeTypeController> logger)
            : base(representativeTypeUseCases, logger)
        {
            _representativeTypeUseCases = representativeTypeUseCases ?? throw new ArgumentNullException(nameof(representativeTypeUseCases));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        protected override GetRepresentativeTypeResponse EntityToDtoMapper(RepresentativeType entity)
        {
            return entity != null ? GetRepresentativeTypeResponse.FromDomain(entity) : throw new ArgumentNullException(nameof(entity));
        }
        
        protected override RepresentativeType CreateRequestToEntity(CreateRepresentativeTypeRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
        
        protected override RepresentativeType UpdateRequestToEntity(UpdateRepresentativeTypeRequest requestDto)
        {
            return requestDto?.ToDomain() ?? throw new ArgumentNullException(nameof(requestDto));
        }
    }
}