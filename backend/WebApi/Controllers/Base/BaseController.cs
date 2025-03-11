using Application.Models;
using Application.UseCases;
using Domain.Entities;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiController]
    // [Authorize]
    public abstract class BaseController<TService, TEntity, TResponseDto, TCreateRequestDto, TUpdateRequestDto> : ControllerBase
        where TService : IBaseUseCases<TEntity>
        where TEntity : class
        where TResponseDto : class
        where TCreateRequestDto : class
        where TUpdateRequestDto : class
    {
        protected readonly TService _service;
        private readonly ILogger _logger;
        
        protected BaseController(TService service, ILogger logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [HttpGet]
        [Route("GetAll")]
        public virtual async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return HandleListDtoResult(result, EntityToDtoMapper);
        }
        
        [HttpGet]
        [Route("GetOneById")]
        public virtual async Task<IActionResult> GetOneById(int id)
        {
            var result = await _service.GetOneByID(id);
            return HandleSingleDtoResult(result, EntityToDtoMapper);
        }
        
        [HttpGet]
        [Route("GetPaginated")]
        public virtual async Task<IActionResult> GetPaginated(int pageSize, int pageNumber)
        {
            var result = await _service.GetPaginated(pageSize, pageNumber);
            return HandlePaginatedDtoResult(result, EntityToDtoMapper);
        }
        
        [HttpPost]
        [Route("Create")]
        public virtual async Task<IActionResult> Create(TCreateRequestDto requestDto)
        {
            var entity = CreateRequestToEntity(requestDto);
            var result = await _service.Create(entity);
            return HandleResult(result, StatusCodes.Status201Created);
        }
        
        [HttpPut]
        [Route("Update")]
        public virtual async Task<IActionResult> Update(TUpdateRequestDto requestDto)
        {
            var entity = UpdateRequestToEntity(requestDto);
            var result = await _service.Update(entity);
            return HandleResult(result);
        }
        
        [HttpDelete]
        [Route("Delete")]
        public virtual async Task<IActionResult> Delete(int id)
        {
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
                return StatusCode(successStatusCode, result.Value);

            return BadRequest(CreateErrorResponse(result));
        }
        
        [NonAction]
        public IActionResult HandleResult(Result result, int successStatusCode = StatusCodes.Status200OK)
        {
            if (result.IsSuccess)
                return StatusCode(successStatusCode);

            return BadRequest(CreateErrorResponse(result));
        }

        [NonAction]
        private object CreateErrorResponse(IResultBase result)
        {
            _logger.LogWarning("Request failed with errors: {Errors}", 
                JsonConvert.SerializeObject(result.Errors, Formatting.Indented, new JsonSerializerSettings 
                { 
                    NullValueHandling = NullValueHandling.Ignore 
                }));
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