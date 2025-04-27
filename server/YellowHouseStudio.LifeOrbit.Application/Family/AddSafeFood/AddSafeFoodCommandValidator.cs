using FluentValidation;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddSafeFood;

public class AddSafeFoodCommandValidator : AbstractValidator<AddSafeFoodCommand>
{
    public AddSafeFoodCommandValidator()
    {
        RuleFor(x => x.FamilyMemberId)
            .NotEmpty();

        RuleFor(x => x.FoodItem)
            .NotEmpty()
            .MaximumLength(100);
    }
}