using Application.UseCases;
using Asp.Versioning;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiController]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerController : BaseController<ICustomerUseCases, Customer, GetCustomerResponse, CreateCustomerRequest,
        UpdateCustomerRequest>
    {
        private readonly ICustomerUseCases _CustomerUseCases;

        public CustomerController(ICustomerUseCases CustomerUseCases,
            ILogger<BaseController<ICustomerUseCases, Customer, GetCustomerResponse, CreateCustomerRequest,
                UpdateCustomerRequest>> logger)
            : base(CustomerUseCases, logger)
        {
            _CustomerUseCases = CustomerUseCases;
        }

        protected override GetCustomerResponse EntityToDtoMapper(Customer entity)
        {
            return GetCustomerResponse.FromDomain(entity);
        }

        protected override Customer CreateRequestToEntity(CreateCustomerRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override Customer UpdateRequestToEntity(UpdateCustomerRequest requestDto)
        {
            return requestDto.ToDomain();
        }

    }
}