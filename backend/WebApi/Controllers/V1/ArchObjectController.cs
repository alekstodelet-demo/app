using FluentResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using Application.UseCases;
using Asp.Versioning;
using Domain.Entities;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;


namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ArchObjectController : BaseController<IArchObjectUseCases, ArchObject, GetArchObjectResponse,
        CreateArchObjectRequest, UpdateArchObjectRequest>
    {
        private readonly IArchObjectUseCases _ArchObjectUseCases;

        public ArchObjectController(IArchObjectUseCases ArchObjectUseCases,
            ILogger<BaseController<IArchObjectUseCases, ArchObject, GetArchObjectResponse, CreateArchObjectRequest,
                UpdateArchObjectRequest>> logger)
            : base(ArchObjectUseCases, logger)
        {
            _ArchObjectUseCases = ArchObjectUseCases;
        }

        protected override GetArchObjectResponse EntityToDtoMapper(ArchObject entity)
        {
            return GetArchObjectResponse.FromDomain(entity);
        }

        protected override ArchObject CreateRequestToEntity(CreateArchObjectRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override ArchObject UpdateRequestToEntity(UpdateArchObjectRequest requestDto)
        {
            return requestDto.ToDomain();
        }
    }
}