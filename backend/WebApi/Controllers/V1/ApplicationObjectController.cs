using FluentResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using Application.UseCases;
using Asp.Versioning;
using Domain.Entities;
using WebApi.Services;


namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApplicationObjectController : BaseController<IApplicationObjectUseCases, ApplicationObject,
        GetApplicationObjectResponse, CreateApplicationObjectRequest, UpdateApplicationObjectRequest>
    {
        private readonly IApplicationObjectUseCases _ApplicationObjectUseCases;
        private readonly ILoggingService _loggingService;

        public ApplicationObjectController(IApplicationObjectUseCases ApplicationObjectUseCases,
            ILogger<BaseController<IApplicationObjectUseCases, ApplicationObject, GetApplicationObjectResponse,
                CreateApplicationObjectRequest, UpdateApplicationObjectRequest>> logger, ILoggingService loggingService)
            : base(ApplicationObjectUseCases, logger, loggingService)
        {
            _ApplicationObjectUseCases = ApplicationObjectUseCases;
        }

        protected override GetApplicationObjectResponse EntityToDtoMapper(ApplicationObject entity)
        {
            return GetApplicationObjectResponse.FromDomain(entity);
        }

        protected override ApplicationObject CreateRequestToEntity(CreateApplicationObjectRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override ApplicationObject UpdateRequestToEntity(UpdateApplicationObjectRequest requestDto)
        {
            return requestDto.ToDomain();
        }
    }
}