using Domain.Entities;
using FluentResults;

namespace Application.Repositories
{
    public interface IRegistrationRepository
    {
        // Существующие методы

        /// <summary>
        /// Проверяет, существует ли международная организация с указанными реквизитами
        /// </summary>
        /// <param name="registrationNumber">Регистрационный номер</param>
        /// <param name="email">Email организации</param>
        /// <returns>true, если организация существует, иначе false</returns>
        Task<Result<bool>> InternationalOrganizationExistsAsync(string registrationNumber, string email);

        /// <summary>
        /// Регистрирует новую международную организацию
        /// </summary>
        /// <param name="request">Данные для регистрации</param>
        /// <returns>Результат регистрации</returns>
        Task<Result<InternationalRegistrationResult>> RegisterInternationalOrganizationAsync(InternationalRegistrationRequest request);
    }
}