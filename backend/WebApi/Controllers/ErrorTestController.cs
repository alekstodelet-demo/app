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
            _logger.LogInformation("Testing 400 Bad Request");
            return BadRequest(new { Message = "This is a sample 400 Bad Request error" });
        }

        [HttpGet("unauthorized")]
        public IActionResult TestUnauthorized()
        {
            _logger.LogInformation("Testing 401 Unauthorized");
            return Unauthorized(new { Message = "This is a sample 401 Unauthorized error" });
        }

        [HttpGet("forbidden")]
        public IActionResult TestForbidden()
        {
            _logger.LogInformation("Testing 403 Forbidden");
            return StatusCode(StatusCodes.Status403Forbidden, new { Message = "This is a sample 403 Forbidden error" });
        }

        [HttpGet("unprocessable-entity")]
        public IActionResult TestUnprocessableEntity()
        {
            _logger.LogInformation("Testing 422 Unprocessable Entity");
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new { Message = "This is a sample 422 Unprocessable Entity error" });
        }

        [HttpGet("fluent-result-error")]
        public IActionResult TestFluentResultError()
        {
            _logger.LogInformation("Testing FluentResult error");
            var result = Result.Fail(new Error("Validation failed")
                .WithMetadata("ErrorCode", "VALIDATION_FAILED")
                .WithMetadata("ErrorType", "Validation Error"));
                
            return ActionResultHelper.FromResult(result, _logger);
        }

        [HttpGet("permission-exception")]
        public IActionResult TestPermissionException()
        {
            _logger.LogInformation("Testing PermissionException");
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
            _logger.LogInformation("Testing ServiceException");
            throw new ServiceException("Failed to connect to external service", new Exception("Connection timeout"));
        }

        [HttpGet("repository-exception")]
        public IActionResult TestRepositoryException()
        {
            _logger.LogInformation("Testing RepositoryException");
            throw new RepositoryException("Failed to query database", new Exception("Timeout expired"));
        }

        [HttpGet("internal-error")]
        public IActionResult TestInternalError()
        {
            _logger.LogInformation("Testing 500 Internal Server Error");
            throw new Exception("This is a sample internal server error");
        }

        [HttpGet("custom-domain-error")]
        public IActionResult TestCustomDomainError()
        {
            _logger.LogInformation("Testing Custom Domain Error");
            var error = new ValidationError("Field validation failed");
            var exception = new Exception("Domain error occurred");
            exception.Data["CustomError"] = error;
            throw exception;
        }
    }
}