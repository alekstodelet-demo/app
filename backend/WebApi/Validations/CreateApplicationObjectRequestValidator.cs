using FluentValidation;
using WebApi.Dtos;
using WebApi.Resources;

namespace WebApi.Validations
{
    /// <summary>
    /// Валидатор для CreateApplicationObjectRequest
    /// </summary>
    public class CreateApplicationObjectRequestValidator : AbstractValidator<CreateApplicationObjectRequest>
    {
        public CreateApplicationObjectRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.ApplicationId)
                .GreaterThan(0).WithMessage(localizer.GetString("ApplicationObject_ApplicationId_Required"));

            RuleFor(x => x.ArchObjectId)
                .GreaterThan(0).WithMessage(localizer.GetString("ApplicationObject_ArchObjectId_Required"));
        }
    }

    /// <summary>
    /// Валидатор для UpdateApplicationObjectRequest
    /// </summary>
    public class UpdateApplicationObjectRequestValidator : AbstractValidator<UpdateApplicationObjectRequest>
    {
        public UpdateApplicationObjectRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(localizer.GetString("ID_Positive"));

            RuleFor(x => x.ApplicationId)
                .GreaterThan(0).WithMessage(localizer.GetString("ApplicationObject_ApplicationId_Required"));

            RuleFor(x => x.ArchObjectId)
                .GreaterThan(0).WithMessage(localizer.GetString("ApplicationObject_ArchObjectId_Required"));
        }
    }
}