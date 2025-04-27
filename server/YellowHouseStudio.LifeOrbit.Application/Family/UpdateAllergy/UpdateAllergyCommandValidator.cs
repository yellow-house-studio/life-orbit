using FluentValidation;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.UpdateAllergy;

public class UpdateAllergyCommandValidator : AbstractValidator<UpdateAllergyCommand>
{
    public UpdateAllergyCommandValidator()
    {
        RuleFor(x => x.FamilyMemberId)
            .NotEmpty();

        RuleFor(x => x.Allergen)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Severity)
            .NotEmpty()
            .Must(severity => Enum.TryParse<AllergySeverity>(severity, out _))
            .WithMessage("Severity must be a valid AllergySeverity value");
    }
}