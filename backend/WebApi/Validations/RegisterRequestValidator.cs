using FluentValidation;
using WebApi.Dtos;
using WebApi.Resources;

namespace WebApi.Validations
{
    /// <summary>
    /// Валидатор для запроса на регистрацию
    /// </summary>
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.EntityType)
                .NotEmpty().WithMessage(localizer.GetString("EntityType_Required"))
                .Must(BeValidEntityType).WithMessage(localizer.GetString("EntityType_Invalid"));

            RuleFor(x => x.Inn)
                .NotEmpty().WithMessage(localizer.GetString("INN_Required"))
                .Must((req, inn) => BeValidInnFormat(inn, req.EntityType))
                .WithMessage((req, _) => req.EntityType == "juridical"
                    ? localizer.GetString("INN_Juridical_Format")
                    : localizer.GetString("INN_Physical_Format"));

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(localizer.GetString("Password_Required"))
                .MinimumLength(8).WithMessage(localizer.GetString("Password_MinLength", 8))
                .Must(HaveValidPasswordComplexity).WithMessage(localizer.GetString("Password_Complexity"));

            RuleFor(x => x.TaxCode)
                .NotEmpty().WithMessage(localizer.GetString("TaxCode_Required"));
        }

        private bool BeValidEntityType(string entityType)
        {
            return entityType == "juridical" || entityType == "physical";
        }

        private bool BeValidInnFormat(string inn, string entityType)
        {
            if (string.IsNullOrEmpty(inn))
                return false;

            // ИНН должен содержать только цифры
            if (!inn.All(char.IsDigit))
                return false;

            // Проверка длины в зависимости от типа сущности
            return entityType == "juridical" ? inn.Length == 10 : inn.Length == 12;
        }

        private bool HaveValidPasswordComplexity(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // Пароль должен содержать хотя бы одну цифру
            var hasDigit = password.Any(char.IsDigit);

            // Пароль должен содержать хотя бы одну букву в верхнем регистре
            var hasUpper = password.Any(char.IsUpper);

            // Пароль должен содержать хотя бы одну букву в нижнем регистре
            var hasLower = password.Any(char.IsLower);

            return hasDigit && (hasUpper || hasLower);
        }
    }
}