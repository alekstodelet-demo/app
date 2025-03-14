using FluentResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using Application.UseCases;
using Domain.Entities;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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