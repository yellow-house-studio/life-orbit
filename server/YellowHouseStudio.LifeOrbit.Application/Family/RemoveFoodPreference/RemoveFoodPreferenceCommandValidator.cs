using FluentValidation;

namespace YellowHouseStudio.LifeOrbit.Application.Family.RemoveFoodPreference;

public class RemoveFoodPreferenceCommandValidator : AbstractValidator<RemoveFoodPreferenceCommand>
{
    public RemoveFoodPreferenceCommandValidator()
    {
        RuleFor(x => x.FamilyMemberId)
            .NotEmpty();

        RuleFor(x => x.FoodItem)
            .NotEmpty()
            .MaximumLength(100);
    }
} 