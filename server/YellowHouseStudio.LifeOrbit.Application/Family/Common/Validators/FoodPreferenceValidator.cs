using FluentValidation;

namespace YellowHouseStudio.LifeOrbit.Application.Family.Common.Validators;

public class FoodPreferenceValidator : AbstractValidator<FoodPreferenceDto>
{
    public FoodPreferenceValidator()
    {
        RuleFor(x => x.FoodItem)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(status => status is "Include" or "AvailableForOthers" or "NotAllowed")
            .WithMessage("Status must be either 'Include', 'AvailableForOthers', or 'NotAllowed'");
    }
}