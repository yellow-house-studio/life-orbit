using FluentValidation;

namespace YellowHouseStudio.LifeOrbit.Application.Family.Common.Validators;

public class AllergyValidator : AbstractValidator<AllergyDto>
{
    public AllergyValidator()
    {
        RuleFor(x => x.Allergen)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Severity)
            .NotEmpty()
            .Must(severity => severity is "AvailableForOthers" or "NotAllowed")
            .WithMessage("Severity must be either 'AvailableForOthers' or 'NotAllowed'");
    }
}