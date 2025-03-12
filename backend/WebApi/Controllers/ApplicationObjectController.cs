using FluentResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using Application.UseCases;
using Domain.Entities;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationObjectController : BaseController<IApplicationObjectUseCases, ApplicationObject,
        GetApplicationObjectResponse, CreateApplicationObjectRequest, UpdateApplicationObjectRequest>
    {
        private readonly IApplicationObjectUseCases _ApplicationObjectUseCases;


        public ApplicationObjectController(IApplicationObjectUseCases ApplicationObjectUseCases,
            ILogger<BaseController<IApplicationObjectUseCases, ApplicationObject, GetApplicationObjectResponse,
                CreateApplicationObjectRequest, UpdateApplicationObjectRequest>> logger)
            : base(ApplicationObjectUseCases, logger)
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