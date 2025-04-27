using FluentValidation;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.UpdateFoodPreference;

public class UpdateFoodPreferenceCommandValidator : AbstractValidator<UpdateFoodPreferenceCommand>
{
    public UpdateFoodPreferenceCommandValidator()
    {
        RuleFor(x => x.FamilyMemberId)
            .NotEmpty();

        RuleFor(x => x.FoodItem)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(status => Enum.TryParse<PreferenceStatus>(status, out _))
            .WithMessage("Status must be a valid PreferenceStatus value");
    }
}