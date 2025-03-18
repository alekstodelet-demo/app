using FluentValidation;
using WebApi.Dtos;
using WebApi.Resources;

namespace WebApi.Validations
{
    /// <summary>
    /// Валидатор для CreateServiceRequest
    /// </summary>
    public class CreateServiceRequestValidator : AbstractValidator<CreateServiceRequest>
    {
        public CreateServiceRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer.GetString("Service_Name_Required"))
                .MaximumLength(100).WithMessage(localizer.GetString("Service_Name_MaxLength", 100));

            RuleFor(x => x.ShortName)
                .MaximumLength(20).WithMessage(localizer.GetString("Service_ShortName_MaxLength", 20));

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(localizer.GetString("Service_Code_Required"))
                .MaximumLength(10).WithMessage(localizer.GetString("Service_Code_MaxLength", 10))
                .Matches("^[A-Z0-9_]*$").WithMessage(localizer.GetString("Service_Code_Format"));

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(localizer.GetString("Description_MaxLength", 500));

            RuleFor(x => x.DayCount)
                .GreaterThanOrEqualTo(0).WithMessage(localizer.GetString("Service_DayCount_Positive"));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage(localizer.GetString("Service_Price_Positive"));
        }
    }

    /// <summary>
    /// Валидатор для UpdateServiceRequest
    /// </summary>
    public class UpdateServiceRequestValidator : AbstractValidator<UpdateServiceRequest>
    {
        public UpdateServiceRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(localizer.GetString("ID_Positive"));

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer.GetString("Service_Name_Required"))
                .MaximumLength(100).WithMessage(localizer.GetString("Service_Name_MaxLength", 100));

            RuleFor(x => x.ShortName)
                .MaximumLength(20).WithMessage(localizer.GetString("Service_ShortName_MaxLength", 20));

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(localizer.GetString("Service_Code_Required"))
                .MaximumLength(10).WithMessage(localizer.GetString("Service_Code_MaxLength", 10))
                .Matches("^[A-Z0-9_]*$").WithMessage(localizer.GetString("Service_Code_Format"));

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(localizer.GetString("Description_MaxLength", 500));

            RuleFor(x => x.DayCount)
                .GreaterThanOrEqualTo(0).WithMessage(localizer.GetString("Service_DayCount_Positive"));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage(localizer.GetString("Service_Price_Positive"));
        }
    }
}