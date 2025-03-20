using FluentValidation;
using WebApi.Dtos;
using WebApi.Resources;
using System.Text.RegularExpressions;

namespace WebApi.Validations
{
    /// <summary>
    /// Валидатор для запроса на регистрацию международной организации
    /// </summary>
    public class RegisterInternationalRequestValidator : AbstractValidator<RegisterInternationalRequest>
    {
        public RegisterInternationalRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.OrganizationName)
                .NotEmpty().WithMessage(localizer.GetString("Organization_Name_Required"))
                .MaximumLength(200).WithMessage(localizer.GetString("Organization_Name_MaxLength", 200));

            RuleFor(x => x.RegistrationNumber)
                .NotEmpty().WithMessage(localizer.GetString("Registration_Number_Required"))
                .MaximumLength(50).WithMessage(localizer.GetString("Registration_Number_MaxLength", 50));

            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage(localizer.GetString("Country_Code_Required"))
                .Length(2).WithMessage(localizer.GetString("Country_Code_Length"))
                .Must(BeValidCountryCode).WithMessage(localizer.GetString("Country_Code_Format"));

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(localizer.GetString("Address_Required"))
                .MaximumLength(500).WithMessage(localizer.GetString("Address_MaxLength", 500));

            RuleFor(x => x.ContactPerson)
                .NotEmpty().WithMessage(localizer.GetString("Contact_Person_Required"))
                .MaximumLength(100).WithMessage(localizer.GetString("Contact_Person_MaxLength", 100));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(localizer.GetString("Email_Required"))
                .EmailAddress().WithMessage(localizer.GetString("Email_Format"))
                .MaximumLength(100).WithMessage(localizer.GetString("Email_MaxLength", 100));

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(localizer.GetString("Phone_Number_Required"))
                .MaximumLength(20).WithMessage(localizer.GetString("Phone_Number_MaxLength", 20))
                .Must(BeValidPhoneNumber).WithMessage(localizer.GetString("Phone_Number_Format"));

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(localizer.GetString("Password_Required"))
                .MinimumLength(8).WithMessage(localizer.GetString("Password_MinLength", 8))
                .Must(HaveValidPasswordComplexity).WithMessage(localizer.GetString("Password_Complexity"));
        }

        private bool BeValidCountryCode(string countryCode)
        {
            if (string.IsNullOrEmpty(countryCode))
                return false;

            // Код страны должен состоять только из букв
            return countryCode.Length == 2 && countryCode.All(char.IsLetter);
        }

        private bool BeValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            // Номер телефона должен начинаться с "+" и содержать минимум 6 цифр
            var regex = new Regex(@"^\+[0-9\s\-\(\)]{6,}$");
            return regex.IsMatch(phoneNumber);
        }

        private bool HaveValidPasswordComplexity(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // Пароль должен содержать хотя бы одну цифру, одну заглавную и одну строчную букву
            var hasDigit = password.Any(char.IsDigit);
            var hasUpper = password.Any(char.IsUpper);
            var hasLower = password.Any(char.IsLower);

            return hasDigit && hasUpper && hasLower;
        }
    }
}