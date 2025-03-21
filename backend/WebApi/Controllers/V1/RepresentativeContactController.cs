using Application.UseCases;
using Application.UseCases.Interfaces;
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
    public class RepresentativeContactController : BaseController<IRepresentativeContactUseCase, RepresentativeContact, GetRepresentativeContactResponse, CreateRepresentativeContactRequest,
        UpdateRepresentativeContactRequest>
    {
        private readonly IRepresentativeContactUseCase _RepresentativeContactUseCase;

        public RepresentativeContactController(IRepresentativeContactUseCase RepresentativeContactUseCase,
            ILogger<BaseController<IRepresentativeContactUseCase, RepresentativeContact, GetRepresentativeContactResponse, CreateRepresentativeContactRequest,
                UpdateRepresentativeContactRequest>> logger)
            : base(RepresentativeContactUseCase, logger)
        {
            _RepresentativeContactUseCase = RepresentativeContactUseCase;
        }

        protected override GetRepresentativeContactResponse EntityToDtoMapper(RepresentativeContact entity)
        {
            return GetRepresentativeContactResponse.FromDomain(entity);
        }

        protected override RepresentativeContact CreateRequestToEntity(CreateRepresentativeContactRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override RepresentativeContact UpdateRequestToEntity(UpdateRepresentativeContactRequest requestDto)
        {
            return requestDto.ToDomain();
        }


        [HttpGet]
        [Route("GetByRepresentativeId")]
        public async Task<IActionResult> GetByRepresentativeId(int RepresentativeId)
        {
            var response = await _RepresentativeContactUseCase.GetByRepresentativeId(RepresentativeId);
            return Ok(response);
        }


    }
}