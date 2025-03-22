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
    public class RepresentativeController : BaseController<IRepresentativeUseCase, Representative, GetRepresentativeResponse, CreateRepresentativeRequest,
        UpdateRepresentativeRequest>
    {
        private readonly IRepresentativeUseCase _RepresentativeUseCase;

        public RepresentativeController(IRepresentativeUseCase RepresentativeUseCase,
            ILogger<BaseController<IRepresentativeUseCase, Representative, GetRepresentativeResponse, CreateRepresentativeRequest,
                UpdateRepresentativeRequest>> logger)
            : base(RepresentativeUseCase, logger)
        {
            _RepresentativeUseCase = RepresentativeUseCase;
        }

        protected override GetRepresentativeResponse EntityToDtoMapper(Representative entity)
        {
            return GetRepresentativeResponse.FromDomain(entity);
        }

        protected override Representative CreateRequestToEntity(CreateRepresentativeRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override Representative UpdateRequestToEntity(UpdateRepresentativeRequest requestDto)
        {
            return requestDto.ToDomain();
        }


        [HttpGet]
        [Route("GetByCompanyId")]
        public async Task<IActionResult> GetByCompanyId(int CompanyId)
        {
            var response = await _RepresentativeUseCase.GetByCompanyId(CompanyId);
            var res = response.Select(x => EntityToDtoMapper(x)).ToList();
            return Ok(res);
        }

        [HttpGet]
        [Route("GetByTypeId")]
        public async Task<IActionResult> GetByTypeId(int TypeId)
        {
            var response = await _RepresentativeUseCase.GetByTypeId(TypeId);
            return Ok(response);
        }

    }
}