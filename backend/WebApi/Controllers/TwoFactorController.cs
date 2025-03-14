using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Repositories;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwoFactorController : ControllerBase
    {
        private readonly ITwoFactorService _twoFactorService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TwoFactorController> _logger;

        public TwoFactorController(
            ITwoFactorService twoFactorService,
            IUnitOfWork unitOfWork,
            ILogger<TwoFactorController> logger)
        {
            _twoFactorService = twoFactorService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("setup")]
        [Authorize]
        public async Task<IActionResult> SetupTwoFactor()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Получаем пользователя из базы данных
            var userRepository = _unitOfWork.UserRepository;
            var userResult = await userRepository.GetOneByID(int.Parse(userId));

            if (userResult.IsFailed)
            {
                return NotFound("User not found");
            }

            var user = userResult.Value;

            // Если 2FA уже настроен, возвращаем ошибку
            if (user.TwoFactorEnabled)
            {
                return BadRequest("Two-factor authentication is already enabled");
            }

            // Генерируем новый секретный ключ
            var secretKey = _twoFactorService.GenerateSecretKey();

            // Сохраняем ключ в сессии, чтобы использовать его при подтверждении
            HttpContext.Session.SetString("TwoFactorSecretKey", secretKey);

            // Создаем URI для QR-кода
            var qrCodeUri = _twoFactorService.GetQrCodeUri(user.Email, secretKey, "BGA-Application");

            return Ok(new
            {
                SecretKey = secretKey,
                QrCodeUri = qrCodeUri
            });
        }

        [HttpPost("verify")]
        [Authorize]
        public async Task<IActionResult> VerifyTwoFactor([FromBody] VerifyTwoFactorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Получаем секретный ключ из сессии
            var secretKey = HttpContext.Session.GetString("TwoFactorSecretKey");
            if (string.IsNullOrEmpty(secretKey))
            {
                return BadRequest("Two-factor setup has not been initiated");
            }

            // Проверяем код
            var isValid = _twoFactorService.ValidateTotpCode(secretKey, request.Code);
            if (!isValid)
            {
                return BadRequest("Invalid verification code");
            }

            // Получаем пользователя из базы данных
            var userRepository = _unitOfWork.UserRepository;
            var userResult = await userRepository.GetOneByID(int.Parse(userId));

            if (userResult.IsFailed)
            {
                return NotFound("User not found");
            }

            var user = userResult.Value;

            // Сохраняем секретный ключ и включаем 2FA
            user.TwoFactorSecret = secretKey;
            user.TwoFactorEnabled = true;

            // Обновляем пользователя в базе данных
            await userRepository.Update(user);
            _unitOfWork.Commit();

            // Очищаем сессию
            HttpContext.Session.Remove("TwoFactorSecretKey");

            return Ok(new { Message = "Two-factor authentication has been enabled" });
        }

        [HttpPost("disable")]
        [Authorize]
        public async Task<IActionResult> DisableTwoFactor([FromBody] VerifyTwoFactorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Получаем пользователя из базы данных
            var userRepository = _unitOfWork.UserRepository;
            var userResult = await userRepository.GetOneByID(int.Parse(userId));

            if (userResult.IsFailed)
            {
                return NotFound("User not found");
            }

            var user = userResult.Value;

            // Проверяем, что 2FA включен
            if (!user.TwoFactorEnabled)
            {
                return BadRequest("Two-factor authentication is not enabled");
            }

            // Проверяем код
            var isValid = _twoFactorService.ValidateTotpCode(user.TwoFactorSecret, request.Code);
            if (!isValid)
            {
                return BadRequest("Invalid verification code");
            }

            // Отключаем 2FA
            user.TwoFactorSecret = null;
            user.TwoFactorEnabled = false;

            // Обновляем пользователя в базе данных
            await userRepository.Update(user);
            _unitOfWork.Commit();

            return Ok(new { Message = "Two-factor authentication has been disabled" });
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateTwoFactor([FromBody] ValidateTwoFactorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userRepository = _unitOfWork.UserRepository;
            var userResult = await userRepository.GetByEmail(request.Email);

            if (userResult.IsFailed)
            {
                _logger.LogWarning("Failed login attempt for non-existent user: {Email}", request.Email);
                return BadRequest("Invalid credentials");
            }

            var user = userResult.Value;

            if (!user.TwoFactorEnabled)
            {
                _logger.LogInformation("User does not have 2FA enabled: {Email}", request.Email);
                return Ok(new ApiResponse<object> { Success = true, Data = new { RequiresTwoFactor = false } });
            }

            if (string.IsNullOrEmpty(user.TwoFactorSecret))
            {
                _logger.LogError("2FA enabled but secret is missing for user: {Email}", request.Email);
                return StatusCode(500, "Server configuration error");
            }

            var isValid = _twoFactorService.ValidateTotpCode(user.TwoFactorSecret, request.Code);
            if (!isValid)
            {
                _logger.LogWarning("Invalid 2FA code provided for user: {Email}", request.Email);
                return BadRequest("Invalid two-factor authentication code");
            }

            _logger.LogInformation("2FA validation successful for user: {Email}", request.Email);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = new { RequiresTwoFactor = true, IsValid = true }
            });
        }
    }
}