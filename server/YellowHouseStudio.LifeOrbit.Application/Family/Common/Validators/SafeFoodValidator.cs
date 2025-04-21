using FluentValidation;

namespace YellowHouseStudio.LifeOrbit.Application.Family.Common.Validators;

public class SafeFoodValidator : AbstractValidator<SafeFoodDto>
{
    public SafeFoodValidator()
    {
        RuleFor(x => x.FoodItem)
            .NotEmpty()
            .MaximumLength(100);
    }
} 