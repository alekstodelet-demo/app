using Domain.Entities;
using FluentResults;

namespace Application.UseCases
{
    public interface IRegistrationService
    {
        // Существующие методы

        /// <summary>
        /// Регистрирует новую международную организацию
        /// </summary>
        /// <param name="request">Данные для регистрации</param>
        /// <returns>Результат регистрации</returns>
        Task<Result<InternationalRegistrationResult>> RegisterInternationalOrganizationAsync(InternationalRegistrationRequest request);
    }
}