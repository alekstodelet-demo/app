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
    public class OrganizationTypeController : BaseController<IOrganizationTypeUseCase, OrganizationType, GetOrganizationTypeResponse, CreateOrganizationTypeRequest,
        UpdateOrganizationTypeRequest>
    {
        private readonly IOrganizationTypeUseCase _OrganizationTypeUseCase;

        public OrganizationTypeController(IOrganizationTypeUseCase OrganizationTypeUseCase,
            ILogger<BaseController<IOrganizationTypeUseCase, OrganizationType, GetOrganizationTypeResponse, CreateOrganizationTypeRequest,
                UpdateOrganizationTypeRequest>> logger)
            : base(OrganizationTypeUseCase, logger)
        {
            _OrganizationTypeUseCase = OrganizationTypeUseCase;
        }

        protected override GetOrganizationTypeResponse EntityToDtoMapper(OrganizationType entity)
        {
            return GetOrganizationTypeResponse.FromDomain(entity);
        }

        protected override OrganizationType CreateRequestToEntity(CreateOrganizationTypeRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override OrganizationType UpdateRequestToEntity(UpdateOrganizationTypeRequest requestDto)
        {
            return requestDto.ToDomain();
        }
        
        

    }
}