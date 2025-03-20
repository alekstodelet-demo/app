using Domain.Entities;
using Application.Repositories;
using FluentResults;
using System.Text.RegularExpressions;

namespace Application.UseCases
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;

        public RegistrationService(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository ?? throw new ArgumentNullException(nameof(registrationRepository));
        }

        // Существующие методы

        public async Task<Result<InternationalRegistrationResult>> RegisterInternationalOrganizationAsync(InternationalRegistrationRequest request)
        {
            // Валидация входных данных
            var validationResult = ValidateInternationalRegistrationRequest(request);
            if (validationResult.IsFailed)
            {
                return validationResult;
            }

            // Проверка, существует ли организация с такими реквизитами
            var existsResult = await _registrationRepository.InternationalOrganizationExistsAsync(
                request.RegistrationNumber, request.Email);

            if (existsResult.IsFailed)
            {
                return Result.Fail(existsResult.Errors);
            }

            if (existsResult.Value)
            {
                return Result.Fail(new Error("Организация с таким регистрационным номером или email уже зарегистрирована")
                    .WithMetadata("ErrorCode", "ORGANIZATION_ALREADY_EXISTS"));
            }

            // Делегируем регистрацию репозиторию
            return await _registrationRepository.RegisterInternationalOrganizationAsync(request);
        }

        private Result ValidateInternationalRegistrationRequest(InternationalRegistrationRequest request)
        {
            var errors = new List<Error>();

            // Валидация обязательных полей
            if (string.IsNullOrWhiteSpace(request.OrganizationName))
            {
                errors.Add(new Error("Название организации не может быть пустым")
                    .WithMetadata("ErrorCode", "EMPTY_ORGANIZATION_NAME")
                    .WithMetadata("Field", "organizationName"));
            }

            if (string.IsNullOrWhiteSpace(request.RegistrationNumber))
            {
                errors.Add(new Error("Регистрационный номер не может быть пустым")
                    .WithMetadata("ErrorCode", "EMPTY_REGISTRATION_NUMBER")
                    .WithMetadata("Field", "registrationNumber"));
            }

            if (string.IsNullOrWhiteSpace(request.CountryCode))
            {
                errors.Add(new Error("Код страны не может быть пустым")
                    .WithMetadata("ErrorCode", "EMPTY_COUNTRY_CODE")
                    .WithMetadata("Field", "countryCode"));
            }
            else if (request.CountryCode.Length != 2)
            {
                errors.Add(new Error("Код страны должен содержать 2 символа в формате ISO")
                    .WithMetadata("ErrorCode", "INVALID_COUNTRY_CODE")
                    .WithMetadata("Field", "countryCode"));
            }

            if (string.IsNullOrWhiteSpace(request.Address))
            {
                errors.Add(new Error("Адрес не может быть пустым")
                    .WithMetadata("ErrorCode", "EMPTY_ADDRESS")
                    .WithMetadata("Field", "address"));
            }

            if (string.IsNullOrWhiteSpace(request.ContactPerson))
            {
                errors.Add(new Error("ФИО контактного лица не может быть пустым")
                    .WithMetadata("ErrorCode", "EMPTY_CONTACT_PERSON")
                    .WithMetadata("Field", "contactPerson"));
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                errors.Add(new Error("Email не может быть пустым")
                    .WithMetadata("ErrorCode", "EMPTY_EMAIL")
                    .WithMetadata("Field", "email"));
            }
            else if (!IsValidEmail(request.Email))
            {
                errors.Add(new Error("Некорректный формат email")
                    .WithMetadata("ErrorCode", "INVALID_EMAIL")
                    .WithMetadata("Field", "email"));
            }

            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                errors.Add(new Error("Номер телефона не может быть пустым")
                    .WithMetadata("ErrorCode", "EMPTY_PHONE_NUMBER")
                    .WithMetadata("Field", "phoneNumber"));
            }
            else if (!IsValidPhoneNumber(request.PhoneNumber))
            {
                errors.Add(new Error("Некорректный формат номера телефона")
                    .WithMetadata("ErrorCode", "INVALID_PHONE_NUMBER")
                    .WithMetadata("Field", "phoneNumber"));
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                errors.Add(new Error("Пароль не может быть пустым")
                    .WithMetadata("ErrorCode", "EMPTY_PASSWORD")
                    .WithMetadata("Field", "password"));
            }
            else if (request.Password.Length < 8)
            {
                errors.Add(new Error("Пароль должен содержать не менее 8 символов")
                    .WithMetadata("ErrorCode", "PASSWORD_TOO_SHORT")
                    .WithMetadata("Field", "password"));
            }
            else if (!IsValidPasswordComplexity(request.Password))
            {
                errors.Add(new Error("Пароль должен содержать хотя бы одну цифру, одну заглавную и одну строчную букву")
                    .WithMetadata("ErrorCode", "INVALID_PASSWORD_COMPLEXITY")
                    .WithMetadata("Field", "password"));
            }

            if (errors.Count > 0)
            {
                return Result.Fail(errors);
            }

            return Result.Ok();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            try
            {
                // Простая проверка: содержит "+" и минимум 6 цифр
                var regex = new Regex(@"^\+[0-9\s\-\(\)]{6,}$");
                return regex.IsMatch(phoneNumber);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPasswordComplexity(string password)
        {
            return password.Any(char.IsDigit) &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower);
        }
    }
}