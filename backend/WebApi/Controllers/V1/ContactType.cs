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
    public class ContactTypeController : BaseController<IContactTypeUseCases, ContactType, GetContactTypeResponse, CreateContactTypeRequest,
        UpdateContactTypeRequest>
    {
        private readonly IContactTypeUseCases _ContactTypeUseCases;

        public ContactTypeController(IContactTypeUseCases ContactTypeUseCases,
            ILogger<BaseController<IContactTypeUseCases, ContactType, GetContactTypeResponse, CreateContactTypeRequest,
                UpdateContactTypeRequest>> logger)
            : base(ContactTypeUseCases, logger)
        {
            _ContactTypeUseCases = ContactTypeUseCases;
        }

        protected override GetContactTypeResponse EntityToDtoMapper(ContactType entity)
        {
            return GetContactTypeResponse.FromDomain(entity);
        }

        protected override ContactType CreateRequestToEntity(CreateContactTypeRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override ContactType UpdateRequestToEntity(UpdateContactTypeRequest requestDto)
        {
            return requestDto.ToDomain();
        }

    }
}