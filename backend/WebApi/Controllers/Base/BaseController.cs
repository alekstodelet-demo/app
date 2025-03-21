﻿using System.Diagnostics;
using System.Text.Json;
using Application.Models;
using Application.UseCases;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Authorize]
    public abstract class
        
        BaseController<TService, TEntity, TResponseDto, TCreateRequestDto, TUpdateRequestDto> : ControllerBase
        where TService : IBaseUseCases<TEntity>
        where TEntity : class
        where TResponseDto : class
        where TCreateRequestDto : class
        where TUpdateRequestDto : class
    {
        protected readonly TService _service;

        private readonly ILogger<BaseController<TService, TEntity, TResponseDto, TCreateRequestDto, TUpdateRequestDto>>
            _logger;

        protected BaseController(TService service,
            ILogger<BaseController<TService, TEntity, TResponseDto, TCreateRequestDto, TUpdateRequestDto>> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        [Route("GetAll")]
        public virtual async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all {EntityType}", typeof(TEntity).Name);
            
            var result = await _service.GetAll();
            return HandleListDtoResult(result, EntityToDtoMapper);
        }

        [HttpGet]
        [Route("GetOneById")]
        public virtual async Task<IActionResult> GetOneById(int id)
        {
            _logger.LogInformation("Getting {EntityType} with ID {Id}", typeof(TEntity).Name, id);
            
            var result = await _service.GetOneByID(id);
            
            return HandleSingleDtoResult(result, EntityToDtoMapper);
        }

        [HttpGet]
        [Route("GetPaginated")]
        public virtual async Task<IActionResult> GetPaginated(int pageSize, int pageNumber)
        {
            _logger.LogInformation("Getting paginated {EntityType} (page {Page}, size {Size})", 
                typeof(TEntity).Name, pageNumber, pageSize);
            
            var result = await _service.GetPaginated(pageSize, pageNumber);
            
            return HandlePaginatedDtoResult(result, EntityToDtoMapper);
        }

        [HttpPost]
        [Route("Create")]
        public virtual async Task<IActionResult> Create(TCreateRequestDto requestDto)
        {
            _logger.LogInformation("Creating new {EntityType}", typeof(TEntity).Name);
            
            var entity = CreateRequestToEntity(requestDto);
            var result = await _service.Create(entity);
            return HandleResult(result, StatusCodes.Status201Created);
        }

        [HttpPut]
        [Route("Update")]
        public virtual async Task<IActionResult> Update(TUpdateRequestDto requestDto)
        {
            _logger.LogInformation("Updating {EntityType}", typeof(TEntity).Name);
            
            var entity = UpdateRequestToEntity(requestDto);
            var result = await _service.Update(entity);
            return HandleResult(result);
        }

        [HttpDelete]
        [Route("Delete")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting {EntityType} with ID {Id}", typeof(TEntity).Name, id);
            
            var result = await _service.Delete(id);
            return HandleResult(result, StatusCodes.Status204NoContent);
        }

        protected abstract TResponseDto EntityToDtoMapper(TEntity entity);
        protected abstract TEntity CreateRequestToEntity(TCreateRequestDto requestDto);
        protected abstract TEntity UpdateRequestToEntity(TUpdateRequestDto requestDto);

        [NonAction]
        private protected IActionResult HandleListDtoResult<TSource, TDto>(
            Result<List<TSource>> result,
            Func<TSource, TDto> mapper)
        {
            if (result.IsSuccess && result.Value != null)
            {
                var dtoResult = result.Value.Select(mapper).ToList();
                return HandleResult(Result.Ok(dtoResult));
            }

            return HandleResult(result);
        }

        [NonAction]
        private protected IActionResult HandleSingleDtoResult<TSource, TDto>(
            Result<TSource> result,
            Func<TSource, TDto> mapper)
        {
            if (result.IsSuccess && result.Value != null)
            {
                var dtoResult = mapper(result.Value);
                var res = JsonConvert.SerializeObject(dtoResult);
                return HandleResult(Result.Ok(dtoResult));
            }

            return HandleResult(result);
        }

        [NonAction]
        private protected IActionResult HandlePaginatedDtoResult<TSource, TDto>(
            Result<PaginatedList<TSource>> result,
            Func<TSource, TDto> mapper)
        {
            if (result.IsSuccess && result.Value != null)
            {
                var paginatedList = result.Value;
                var mappedItems = paginatedList.Items.Select(mapper).ToList();

                var dtoResult = new PaginatedList<TDto>(
                    items: mappedItems,
                    count: paginatedList.TotalCount,
                    pageNumber: paginatedList.PageNumber,
                    pageSize: paginatedList.PageSize
                );
                return HandleResult(Result.Ok(dtoResult));
            }

            return HandleResult(result);
        }

        [NonAction]
        public IActionResult HandleResult<T>(Result<T> result, int successStatusCode = StatusCodes.Status200OK)
        {
            if (result.IsSuccess)
            {
                return StatusCode(successStatusCode, result.Value);
            }

            return BadRequest(CreateErrorResponse(result));
        }

        [NonAction]
        public IActionResult HandleResult(Result result, int successStatusCode = StatusCodes.Status200OK)
        {
            if (result.IsSuccess)
            {
                return StatusCode(successStatusCode);
            }

            return BadRequest(CreateErrorResponse(result));
        }

        [NonAction]
        private object CreateErrorResponse(IResultBase result)
        {
            foreach (var error in result.Errors)
            {
                if (error is ExceptionalError exceptionalError && exceptionalError.Exception != null)
                {
                    _logger.LogError(exceptionalError.Exception, "Error occurred with exception: {Message}",
                        exceptionalError.Message);
                }
                else
                {
                    _logger.LogError("Error occurred: {Message}", error.Message);
                }
            }

            return new
            {
                Errors = result.Errors.Select(e => new
                {
                    Message = e.Message,
                    Code = e.Metadata.TryGetValue("ErrorCode", out var code) ? code?.ToString() : "UNKNOWN"
                })
            };
        }
    }
}