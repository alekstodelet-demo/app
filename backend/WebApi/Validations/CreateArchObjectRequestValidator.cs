using FluentValidation;
using WebApi.Dtos;
using WebApi.Resources;

namespace WebApi.Validations
{
    /// <summary>
    /// Валидатор для CreateArchObjectRequest
    /// </summary>
    public class CreateArchObjectRequestValidator : AbstractValidator<CreateArchObjectRequest>
    {
        public CreateArchObjectRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(localizer.GetString("ArchObject_Address_Required"))
                .MaximumLength(500).WithMessage(localizer.GetString("ArchObject_Address_MaxLength", 500));

            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage(localizer.GetString("ArchObject_Name_MaxLength", 200))
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Identifier)
                .MaximumLength(50).WithMessage(localizer.GetString("ArchObject_Identifier_MaxLength", 50))
                .When(x => !string.IsNullOrEmpty(x.Identifier));

            RuleFor(x => x.DistrictId)
                .GreaterThan(0).WithMessage(localizer.GetString("ArchObject_DistrictId_Positive"))
                .When(x => x.DistrictId.HasValue);

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage(localizer.GetString("ArchObject_Description_MaxLength", 1000))
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.XCoordinate)
                .Must(BeValidLongitude).WithMessage(localizer.GetString("ArchObject_XCoordinate_Range"))
                .When(x => x.XCoordinate.HasValue);

            RuleFor(x => x.YCoordinate)
                .Must(BeValidLatitude).WithMessage(localizer.GetString("ArchObject_YCoordinate_Range"))
                .When(x => x.YCoordinate.HasValue);
        }

        private bool BeValidLongitude(double? longitude)
        {
            return longitude == null || (longitude >= -180 && longitude <= 180);
        }

        private bool BeValidLatitude(double? latitude)
        {
            return latitude == null || (latitude >= -90 && latitude <= 90);
        }
    }

    /// <summary>
    /// Валидатор для UpdateArchObjectRequest
    /// </summary>
    public class UpdateArchObjectRequestValidator : AbstractValidator<UpdateArchObjectRequest>
    {
        public UpdateArchObjectRequestValidator(IValidationLocalizerService localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(localizer.GetString("ID_Positive"));

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(localizer.GetString("ArchObject_Address_Required"))
                .MaximumLength(500).WithMessage(localizer.GetString("ArchObject_Address_MaxLength", 500));

            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage(localizer.GetString("ArchObject_Name_MaxLength", 200))
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Identifier)
                .MaximumLength(50).WithMessage(localizer.GetString("ArchObject_Identifier_MaxLength", 50))
                .When(x => !string.IsNullOrEmpty(x.Identifier));

            RuleFor(x => x.DistrictId)
                .GreaterThan(0).WithMessage(localizer.GetString("ArchObject_DistrictId_Positive"))
                .When(x => x.DistrictId.HasValue);

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage(localizer.GetString("ArchObject_Description_MaxLength", 1000))
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.XCoordinate)
                .Must(BeValidLongitude).WithMessage(localizer.GetString("ArchObject_XCoordinate_Range"))
                .When(x => x.XCoordinate.HasValue);

            RuleFor(x => x.YCoordinate)
                .Must(BeValidLatitude).WithMessage(localizer.GetString("ArchObject_YCoordinate_Range"))
                .When(x => x.YCoordinate.HasValue);
        }

        private bool BeValidLongitude(double? longitude)
        {
            return longitude == null || (longitude >= -180 && longitude <= 180);
        }

        private bool BeValidLatitude(double? latitude)
        {
            return latitude == null || (latitude >= -90 && latitude <= 90);
        }
    }
}