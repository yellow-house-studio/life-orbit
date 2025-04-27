using FluentValidation;

namespace YellowHouseStudio.LifeOrbit.Application.Family.RemoveSafeFood;

public class RemoveSafeFoodCommandValidator : AbstractValidator<RemoveSafeFoodCommand>
{
    public RemoveSafeFoodCommandValidator()
    {
        RuleFor(x => x.FamilyMemberId)
            .NotEmpty();

        RuleFor(x => x.FoodItem)
            .NotEmpty()
            .MaximumLength(100);
    }
}