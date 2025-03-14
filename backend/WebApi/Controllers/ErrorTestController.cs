using Application.Exceptions;
using Domain;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorTestController : ControllerBase
    {
        private readonly ILogger<ErrorTestController> _logger;

        public ErrorTestController(ILogger<ErrorTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("bad-request")]
        public IActionResult TestBadRequest()
        {
            _logger.LogError("Testing 400 Bad Request");
            return BadRequest(new { Message = "This is a sample 400 Bad Request error" });
        }

        [HttpGet("unauthorized")]
        public IActionResult TestUnauthorized()
        {
            _logger.LogError("Testing 401 Unauthorized");
            return Unauthorized(new { Message = "This is a sample 401 Unauthorized error" });
        }

        [HttpGet("forbidden")]
        public IActionResult TestForbidden()
        {
            _logger.LogError("Testing 403 Forbidden");
            return StatusCode(StatusCodes.Status403Forbidden, new { Message = "This is a sample 403 Forbidden error" });
        }

        [HttpGet("unprocessable-entity")]
        public IActionResult TestUnprocessableEntity()
        {
            _logger.LogError("Testing 422 Unprocessable Entity");
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new { Message = "This is a sample 422 Unprocessable Entity error" });
        }

        [HttpGet("fluent-result-error")]
        public IActionResult TestFluentResultError()
        {
            _logger.LogError("Testing FluentResult error");
            var result = Result.Fail(new Error("Validation failed")
                .WithMetadata("ErrorCode", "VALIDATION_FAILED")
                .WithMetadata("ErrorType", "Validation Error"));
                
            return ActionResultHelper.FromResult(result, _logger);
        }

        [HttpGet("permission-exception")]
        public IActionResult TestPermissionException()
        {
            _logger.LogError("Testing PermissionException");
            throw new PermissionException("You don't have permission to access this resource", 
                new PermissionExceptionDetails { 
                    Code = "NO_PERMISSION", 
                    Role = "Admin" 
                }, 
                null);
        }

        [HttpGet("service-exception")]
        public IActionResult TestServiceException()
        {
            _logger.LogError("Testing ServiceException");
            throw new ServiceException("Failed to connect to external service", new Exception("Connection timeout"));
        }

        [HttpGet("repository-exception")]
        public IActionResult TestRepositoryException()
        {
            _logger.LogError("Testing RepositoryException");
            throw new RepositoryException("Failed to query database", new Exception("Timeout expired"));
        }

        [HttpGet("internal-error")]
        public IActionResult TestInternalError()
        {
            _logger.LogError("Testing 500 Internal Server Error");
            throw new Exception("This is a sample internal server error");
        }

        [HttpGet("custom-domain-error")]
        public IActionResult TestCustomDomainError()
        {
            _logger.LogError("Testing Custom Domain Error");
            var error = new ValidationError("Field validation failed");
            var exception = new Exception("Domain error occurred");
            exception.Data["CustomError"] = error;
            throw exception;
        }

        [HttpGet("not-found")]
        public IActionResult TestNotFound()
        {
            _logger.LogError("Testing 404 Not Found");
            return NotFound(new { Message = "This is a test 404 Not Found error" });
        }

        [HttpGet("fluent-result-validation")]
        public IActionResult TestFluentResultValidation()
        {
            _logger.LogError("Testing FluentResult validation error");
            var result = Result.Fail(new Error("Validation failed")
                .WithMetadata("ErrorCode", "VALIDATION_FAILED")
                .WithMetadata("ErrorType", "Validation Error"));
                
            return ActionResultHelper.FromResult(result, _logger);
        }

        [HttpGet("fluent-result-not-found")]
        public IActionResult TestFluentResultNotFound()
        {
            _logger.LogError("Testing FluentResult not found error");
            var result = Result.Fail(new Error("Resource not found")
                .WithMetadata("ErrorCode", "NOT_FOUND")
                .WithMetadata("ErrorType", "Not Found Error"));
                
            return ActionResultHelper.FromResult(result, _logger);
        }

        [HttpGet("validation-error")]
        public IActionResult TestValidationError()
        {
            _logger.LogError("Testing CustomError - ValidationError");
            var error = new ValidationError("Field validation failed");
            var exception = new Exception("Domain error occurred");
            exception.Data["CustomError"] = error;
            throw exception;
        }
    }
}