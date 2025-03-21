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
    public class RepresentativeTypeController : BaseController<IRepresentativeTypeUseCase, RepresentativeType, GetRepresentativeTypeResponse, CreateRepresentativeTypeRequest,
        UpdateRepresentativeTypeRequest>
    {
        private readonly IRepresentativeTypeUseCase _RepresentativeTypeUseCase;

        public RepresentativeTypeController(IRepresentativeTypeUseCase RepresentativeTypeUseCase,
            ILogger<BaseController<IRepresentativeTypeUseCase, RepresentativeType, GetRepresentativeTypeResponse, CreateRepresentativeTypeRequest,
                UpdateRepresentativeTypeRequest>> logger)
            : base(RepresentativeTypeUseCase, logger)
        {
            _RepresentativeTypeUseCase = RepresentativeTypeUseCase;
        }

        protected override GetRepresentativeTypeResponse EntityToDtoMapper(RepresentativeType entity)
        {
            return GetRepresentativeTypeResponse.FromDomain(entity);
        }

        protected override RepresentativeType CreateRequestToEntity(CreateRepresentativeTypeRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override RepresentativeType UpdateRequestToEntity(UpdateRepresentativeTypeRequest requestDto)
        {
            return requestDto.ToDomain();
        }
        
        

    }
}