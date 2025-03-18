using FluentValidation;
using WebApi.Dtos;
using WebApi.Resources;

namespace WebApi.Validations
{
    /// <summary>
    /// Валидатор для LoginRequest
    /// </summary>
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.Pin)
                .NotEmpty().WithMessage(localizer.GetString("PIN_Required"))
                .MinimumLength(4).WithMessage(localizer.GetString("PIN_MinLength"))
                .MaximumLength(20).WithMessage(localizer.GetString("PIN_MaxLength"));

            RuleFor(x => x.TokenId)
                .NotEmpty().WithMessage(localizer.GetString("TokenId_Required"))
                .MaximumLength(100).WithMessage(localizer.GetString("TokenId_MaxLength"));

            RuleFor(x => x.Signature)
                .NotEmpty().WithMessage(localizer.GetString("Signature_Required"))
                .MaximumLength(500).WithMessage(localizer.GetString("Signature_MaxLength"));

            RuleFor(x => x.DeviceId)
                .MaximumLength(100).WithMessage(localizer.GetString("DeviceId_MaxLength"))
                .When(x => !string.IsNullOrEmpty(x.DeviceId));
        }
    }

    /// <summary>
    /// Валидатор для RefreshTokenRequest
    /// </summary>
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage(localizer.GetString("RefreshToken_Required"))
                .MinimumLength(10).WithMessage(localizer.GetString("RefreshToken_MinLength"))
                .MaximumLength(500).WithMessage(localizer.GetString("RefreshToken_MaxLength"));

            RuleFor(x => x.DeviceId)
                .MaximumLength(100).WithMessage(localizer.GetString("DeviceId_MaxLength"))
                .When(x => !string.IsNullOrEmpty(x.DeviceId));
        }
    }

    /// <summary>
    /// Валидатор для RevokeTokenRequest
    /// </summary>
    public class RevokeTokenRequestValidator : AbstractValidator<RevokeTokenRequest>
    {
        public RevokeTokenRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage(localizer.GetString("RefreshToken_Required"))
                .MinimumLength(10).WithMessage(localizer.GetString("RefreshToken_MinLength"))
                .MaximumLength(500).WithMessage(localizer.GetString("RefreshToken_MaxLength"));
        }
    }

    /// <summary>
    /// Валидатор для VerifyTwoFactorRequest
    /// </summary>
    public class VerifyTwoFactorRequestValidator : AbstractValidator<VerifyTwoFactorRequest>
    {
        public VerifyTwoFactorRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(localizer.GetString("TwoFactorCode_Required"))
                .Length(6).WithMessage(localizer.GetString("TwoFactorCode_Length"))
                .Matches("^\\d{6}$").WithMessage(localizer.GetString("TwoFactorCode_Format"));
        }
    }

    /// <summary>
    /// Валидатор для ValidateTwoFactorRequest
    /// </summary>
    public class ValidateTwoFactorRequestValidator : AbstractValidator<ValidateTwoFactorRequest>
    {
        public ValidateTwoFactorRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(localizer.GetString("Email_Required"))
                .EmailAddress().WithMessage(localizer.GetString("Email_Format"));

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(localizer.GetString("TwoFactorCode_Required"))
                .Length(6).WithMessage(localizer.GetString("TwoFactorCode_Length"))
                .Matches("^\\d{6}$").WithMessage(localizer.GetString("TwoFactorCode_Format"));
        }
    }
}