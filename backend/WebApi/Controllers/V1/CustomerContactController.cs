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
    public class CustomerContactController : BaseController<ICustomerContactUseCases, CustomerContact, GetCustomerContactResponse, CreateCustomerContactRequest,
        UpdateCustomerContactRequest>
    {
        private readonly ICustomerContactUseCases _CustomerContactUseCase;

        public CustomerContactController(ICustomerContactUseCases CustomerContactUseCase,
            ILogger<BaseController<ICustomerContactUseCases, CustomerContact, GetCustomerContactResponse, CreateCustomerContactRequest,
                UpdateCustomerContactRequest>> logger)
            : base(CustomerContactUseCase, logger)
        {
            _CustomerContactUseCase = CustomerContactUseCase;
        }

        protected override GetCustomerContactResponse EntityToDtoMapper(CustomerContact entity)
        {
            return GetCustomerContactResponse.FromDomain(entity);
        }

        protected override CustomerContact CreateRequestToEntity(CreateCustomerContactRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override CustomerContact UpdateRequestToEntity(UpdateCustomerContactRequest requestDto)
        {
            return requestDto.ToDomain();
        }
        
        
        [HttpGet]
        [Route("GetByOrganizationId")]
        public async Task<IActionResult> GetByOrganizationId(int OrganizationId)
        {
            var response = await _CustomerContactUseCase.GetByOrganizationId(OrganizationId);
            var res = response.Select(x => EntityToDtoMapper(x)).ToList();
            return Ok(res);
        }
        

    }
}