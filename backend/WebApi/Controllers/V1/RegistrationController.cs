using Application.UseCases;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using FluentValidation;
using System.Collections.Generic;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<RegistrationController> _logger;
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly IValidator<RegisterInternationalRequest> _registerInternationalValidator;

        public RegistrationController(
            IRegistrationService registrationService,
            ILogger<RegistrationController> logger,
            IValidator<RegisterRequest> registerValidator,
            IValidator<RegisterInternationalRequest> registerInternationalValidator)
        {
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _registerValidator = registerValidator ?? throw new ArgumentNullException(nameof(registerValidator));
            _registerInternationalValidator = registerInternationalValidator ?? throw new ArgumentNullException(nameof(registerInternationalValidator));
        }

        // Существующие методы

        /// <summary>
        /// Регистрирует новую международную организацию
        /// </summary>
        [HttpPost("register-international")]
        public async Task<IActionResult> RegisterInternational([FromBody] RegisterInternationalRequest request)
        {
            _logger.LogInformation("Запрос на регистрацию международной организации: {Name}, {RegistrationNumber}",
                request.OrganizationName, request.RegistrationNumber);

            // Валидация запроса с помощью FluentValidation
            var validationResult = await _registerInternationalValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var validationErrors = new Dictionary<string, string[]>();
                foreach (var error in validationResult.Errors)
                {
                    if (!validationErrors.ContainsKey(error.PropertyName))
                    {
                        validationErrors[error.PropertyName] = new string[] { error.ErrorMessage };
                    }
                    else
                    {
                        var errors = validationErrors[error.PropertyName].ToList();
                        errors.Add(error.ErrorMessage);
                        validationErrors[error.PropertyName] = errors.ToArray();
                    }
                }

                var response = new ApiErrorResponseWithDetails<Dictionary<string, string[]>>
                {
                    Error = new ApiErrorResponse.ErrorDetails
                    {
                        Code = "VALIDATION_ERROR",
                        Message = "Ошибка валидации"
                    },
                    Details = validationErrors
                };

                return BadRequest(response);
            }

            // Регистрация международной организации
            var registrationResult = await _registrationService.RegisterInternationalOrganizationAsync(request.ToDomain());

            if (registrationResult.IsFailed)
            {
                var errors = registrationResult.Errors;

                // Если ошибки связаны с валидацией полей, формируем подробный ответ
                if (errors.Any(e => e.Metadata.ContainsKey("Field")))
                {
                    var validationErrors = new Dictionary<string, string[]>();
                    foreach (var err in errors.Where(e => e.Metadata.ContainsKey("Field")))
                    {
                        var field = err.Metadata["Field"].ToString();
                        if (!validationErrors.ContainsKey(field))
                        {
                            validationErrors[field] = new string[] { err.Message };
                        }
                        else
                        {
                            var fieldErrors = validationErrors[field].ToList();
                            fieldErrors.Add(err.Message);
                            validationErrors[field] = fieldErrors.ToArray();
                        }
                    }

                    var response = new ApiErrorResponseWithDetails<Dictionary<string, string[]>>
                    {
                        Error = new ApiErrorResponse.ErrorDetails
                        {
                            Code = "VALIDATION_ERROR",
                            Message = "Ошибка валидации"
                        },
                        Details = validationErrors
                    };

                    return BadRequest(response);
                }

                // Если ошибки не связаны с валидацией полей, формируем общий ответ
                var error = errors.First();
                var errorCode = error.Metadata.TryGetValue("ErrorCode", out var code)
                    ? code?.ToString()
                    : "UNKNOWN_ERROR";

                var generalResponse = new ApiErrorResponse
                {
                    Error = new ApiErrorResponse.ErrorDetails
                    {
                        Code = errorCode,
                        Message = error.Message
                    }
                };

                // Определяем HTTP-статус на основе кода ошибки
                var statusCode = errorCode switch
                {
                    "ORGANIZATION_ALREADY_EXISTS" => StatusCodes.Status409Conflict,
                    "EMPTY_ORGANIZATION_NAME" => StatusCodes.Status400BadRequest,
                    "EMPTY_REGISTRATION_NUMBER" => StatusCodes.Status400BadRequest,
                    "EMPTY_COUNTRY_CODE" => StatusCodes.Status400BadRequest,
                    "INVALID_COUNTRY_CODE" => StatusCodes.Status400BadRequest,
                    "EMPTY_ADDRESS" => StatusCodes.Status400BadRequest,
                    "EMPTY_CONTACT_PERSON" => StatusCodes.Status400BadRequest,
                    "EMPTY_EMAIL" => StatusCodes.Status400BadRequest,
                    "INVALID_EMAIL" => StatusCodes.Status400BadRequest,
                    "EMPTY_PHONE_NUMBER" => StatusCodes.Status400BadRequest,
                    "INVALID_PHONE_NUMBER" => StatusCodes.Status400BadRequest,
                    "EMPTY_PASSWORD" => StatusCodes.Status400BadRequest,
                    "PASSWORD_TOO_SHORT" => StatusCodes.Status400BadRequest,
                    "INVALID_PASSWORD_COMPLEXITY" => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCode, generalResponse);
            }

            var result = registrationResult.Value;
            var responseData = RegisterInternationalResponse.FromDomain(result);

            return Created(result.RedirectUrl, new ApiSuccessResponse<RegisterInternationalResponse>
            {
                Data = responseData
            });
        }

    }
}