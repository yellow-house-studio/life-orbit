using FluentValidation;

namespace YellowHouseStudio.LifeOrbit.Application.Family.RemoveAllergy;

public class RemoveAllergyCommandValidator : AbstractValidator<RemoveAllergyCommand>
{
    public RemoveAllergyCommandValidator()
    {
        RuleFor(x => x.FamilyMemberId)
            .NotEmpty();

        RuleFor(x => x.Allergen)
            .NotEmpty()
            .MaximumLength(100);
    }
} 