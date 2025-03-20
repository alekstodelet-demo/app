using System.Data;
using Application.Repositories;
using Domain.Entities;
using FluentResults;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        // Существующие методы

        public async Task<Result<bool>> InternationalOrganizationExistsAsync(string registrationNumber, string email)
        {
            try
            {
                // В реальной системе здесь будет запрос в базу данных
                // Для демонстрации мы возвращаем тестовые данные

                // Допустим, что организации с регистрационным номером, заканчивающимся на "0", уже существуют
                return Result.Ok(registrationNumber.EndsWith("0") || email.Contains("existing"));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Ошибка при проверке существования организации", ex)
                    .WithMetadata("ErrorCode", "ORGANIZATION_EXISTS_CHECK_FAILED"));
            }
        }

        public async Task<Result<InternationalRegistrationResult>> RegisterInternationalOrganizationAsync(InternationalRegistrationRequest request)
        {
            try
            {
                // В реальной системе здесь будет запрос в базу данных для регистрации организации
                // Для демонстрации мы возвращаем тестовые данные

                // Генерация хеша пароля (в реальной системе нужно использовать более безопасные методы)
                var passwordHash = HashPassword(request.Password);

                // Создание JWT-токена (в реальной системе токен должен генерироваться безопасно)
                var token = GenerateJwtToken(request.OrganizationName);

                // Создание ID организации (в реальной системе ID будет генерироваться базой данных)
                var organizationId = "INT" + Guid.NewGuid().ToString("N").Substring(0, 5);

                // Все международные организации проходят обязательную верификацию
                var status = "pending_verification";

                return Result.Ok(new InternationalRegistrationResult
                {
                    OrganizationId = organizationId,
                    Token = token,
                    ExpiresIn = 3600, // 1 час
                    RedirectUrl = "/international-dashboard",
                    Status = status
                });
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Ошибка при регистрации международной организации", ex)
                    .WithMetadata("ErrorCode", "INTERNATIONAL_REGISTRATION_FAILED"));
            }
        }
        private string HashPassword(string password)
        {
            // В реальной системе нужно использовать безопасные методы хеширования паролей
            // например, BCrypt или PBKDF2
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private string GenerateJwtToken(string inn)
        {
            // В реальной системе токен должен генерироваться безопасно с помощью
            // специализированных библиотек для работы с JWT
            // Это просто заглушка для демонстрации
            return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        }

        // Методы для хеширования пароля и генерации токена остаются без изменений
    }
}