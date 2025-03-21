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
    public class CustomerRequisiteController : BaseController<ICustomerRequisiteUseCase, CustomerRequisite, GetCustomerRequisiteResponse, CreateCustomerRequisiteRequest,
        UpdateCustomerRequisiteRequest>
    {
        private readonly ICustomerRequisiteUseCase _CustomerRequisiteUseCase;

        public CustomerRequisiteController(ICustomerRequisiteUseCase CustomerRequisiteUseCase,
            ILogger<BaseController<ICustomerRequisiteUseCase, CustomerRequisite, GetCustomerRequisiteResponse, CreateCustomerRequisiteRequest,
                UpdateCustomerRequisiteRequest>> logger)
            : base(CustomerRequisiteUseCase, logger)
        {
            _CustomerRequisiteUseCase = CustomerRequisiteUseCase;
        }

        protected override GetCustomerRequisiteResponse EntityToDtoMapper(CustomerRequisite entity)
        {
            return GetCustomerRequisiteResponse.FromDomain(entity);
        }

        protected override CustomerRequisite CreateRequestToEntity(CreateCustomerRequisiteRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override CustomerRequisite UpdateRequestToEntity(UpdateCustomerRequisiteRequest requestDto)
        {
            return requestDto.ToDomain();
        }
        
        
        [HttpGet]
        [Route("GetByOrganizationId")]
        public async Task<IActionResult> GetByOrganizationId(int OrganizationId)
        {
            var response = await _CustomerRequisiteUseCase.GetByOrganizationId(OrganizationId);
            return Ok(response);
        }
        

    }
}