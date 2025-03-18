using FluentValidation;
using WebApi.Dtos;
using WebApi.Resources;

namespace WebApi.Validations
{
    /// <summary>
    /// Валидатор для CreateApplicationRequest
    /// </summary>
    public class CreateApplicationRequestValidator : AbstractValidator<CreateApplicationRequest>
    {
        public CreateApplicationRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage(localizer.GetString("Application_CustomerId_Required"));

            RuleFor(x => x.ServiceId)
                .GreaterThan(0).WithMessage(localizer.GetString("Application_ServiceId_Required"));

            RuleFor(x => x.StatusId)
                .GreaterThan(0).WithMessage(localizer.GetString("Application_StatusId_Required"));

            RuleFor(x => x.WorkflowId)
                .GreaterThan(0).WithMessage(localizer.GetString("Application_WorkflowId_Required"));

            RuleFor(x => x.Number)
                .MaximumLength(50).WithMessage(localizer.GetString("Application_Number_MaxLength", 50));

            RuleFor(x => x.CustomerName)
                .MaximumLength(200).WithMessage(localizer.GetString("Application_CustomerName_MaxLength", 200))
                .When(x => !string.IsNullOrEmpty(x.CustomerName));

            RuleFor(x => x.CustomerPin)
                .MaximumLength(20).WithMessage(localizer.GetString("Application_CustomerPin_MaxLength", 20))
                .When(x => !string.IsNullOrEmpty(x.CustomerPin));

            RuleFor(x => x.Deadline)
                .Must(BeValidFutureDate).WithMessage(localizer.GetString("Application_Deadline_Future"))
                .When(x => x.Deadline.HasValue);

            RuleFor(x => x.CustomerAddress)
                .MaximumLength(500).WithMessage(localizer.GetString("Application_CustomerAddress_MaxLength", 500))
                .When(x => !string.IsNullOrEmpty(x.CustomerAddress));

            RuleFor(x => x.CustomerOkpo)
                .MaximumLength(15).WithMessage(localizer.GetString("Application_CustomerOkpo_MaxLength", 15))
                .When(x => !string.IsNullOrEmpty(x.CustomerOkpo));

            RuleFor(x => x.CustomerDirector)
                .MaximumLength(200).WithMessage(localizer.GetString("Application_CustomerDirector_MaxLength", 200))
                .When(x => !string.IsNullOrEmpty(x.CustomerDirector));
        }

        private bool BeValidFutureDate(DateTime? date)
        {
            return date == null || (date > DateTime.Now && date < DateTime.Now.AddYears(10));
        }
    }

    /// <summary>
    /// Валидатор для UpdateApplicationRequest
    /// </summary>
    public class UpdateApplicationRequestValidator : AbstractValidator<UpdateApplicationRequest>
    {
        public UpdateApplicationRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(localizer.GetString("ID_Positive"));

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage(localizer.GetString("Application_CustomerId_Required"));

            RuleFor(x => x.ServiceId)
                .GreaterThan(0).WithMessage(localizer.GetString("Application_ServiceId_Required"));

            RuleFor(x => x.StatusId)
                .GreaterThan(0).WithMessage(localizer.GetString("Application_StatusId_Required"));

            RuleFor(x => x.WorkflowId)
                .GreaterThan(0).WithMessage(localizer.GetString("Application_WorkflowId_Required"));

            RuleFor(x => x.Number)
                .MaximumLength(50).WithMessage(localizer.GetString("Application_Number_MaxLength", 50));

            RuleFor(x => x.CustomerName)
                .MaximumLength(200).WithMessage(localizer.GetString("Application_CustomerName_MaxLength", 200))
                .When(x => !string.IsNullOrEmpty(x.CustomerName));

            RuleFor(x => x.CustomerPin)
                .MaximumLength(20).WithMessage(localizer.GetString("Application_CustomerPin_MaxLength", 20))
                .When(x => !string.IsNullOrEmpty(x.CustomerPin));

            RuleFor(x => x.Deadline)
                .Must(BeValidFutureDate).WithMessage(localizer.GetString("Application_Deadline_Future"))
                .When(x => x.Deadline.HasValue);

            RuleFor(x => x.CustomerAddress)
                .MaximumLength(500).WithMessage(localizer.GetString("Application_CustomerAddress_MaxLength", 500))
                .When(x => !string.IsNullOrEmpty(x.CustomerAddress));

            RuleFor(x => x.CustomerOkpo)
                .MaximumLength(15).WithMessage(localizer.GetString("Application_CustomerOkpo_MaxLength", 15))
                .When(x => !string.IsNullOrEmpty(x.CustomerOkpo));

            RuleFor(x => x.CustomerDirector)
                .MaximumLength(200).WithMessage(localizer.GetString("Application_CustomerDirector_MaxLength", 200))
                .When(x => !string.IsNullOrEmpty(x.CustomerDirector));
        }

        private bool BeValidFutureDate(DateTime? date)
        {
            return date == null || (date > DateTime.Now && date < DateTime.Now.AddYears(10));
        }
    }
}