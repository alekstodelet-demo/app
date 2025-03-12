using FluentResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using Application.UseCases;
using Domain.Entities;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : BaseController<IApplicationUseCases, Domain.Entities.Application,
        GetApplicationResponse, CreateApplicationRequest, UpdateApplicationRequest>
    {
        private readonly IApplicationUseCases _ApplicationUseCases;

        public ApplicationController(IApplicationUseCases ApplicationUseCases,
            ILogger<BaseController<IApplicationUseCases, Domain.Entities.Application, GetApplicationResponse,
                CreateApplicationRequest, UpdateApplicationRequest>> logger)
            : base(ApplicationUseCases, logger)
        {
            _ApplicationUseCases = ApplicationUseCases;
        }

        protected override GetApplicationResponse EntityToDtoMapper(Domain.Entities.Application entity)
        {
            return GetApplicationResponse.FromDomain(entity);
        }

        protected override Domain.Entities.Application CreateRequestToEntity(CreateApplicationRequest requestDto)
        {
            return requestDto.ToDomain();
        }

        protected override Domain.Entities.Application UpdateRequestToEntity(UpdateApplicationRequest requestDto)
        {
            return requestDto.ToDomain();
        }
    }
}