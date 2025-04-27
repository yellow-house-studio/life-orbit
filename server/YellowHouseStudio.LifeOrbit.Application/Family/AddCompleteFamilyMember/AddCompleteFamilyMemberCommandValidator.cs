using FluentValidation;
using YellowHouseStudio.LifeOrbit.Application.Family.Common.Validators;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddCompleteFamilyMember;

public class AddCompleteFamilyMemberCommandValidator : AbstractValidator<AddCompleteFamilyMemberCommand>
{
    public AddCompleteFamilyMemberCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Age)
            .InclusiveBetween(0, 120)
            .WithMessage("Age must be between 0 and 120");

        RuleForEach(x => x.Allergies)
            .SetValidator(new AllergyValidator());

        RuleForEach(x => x.SafeFoods)
            .SetValidator(new SafeFoodValidator());

        RuleForEach(x => x.FoodPreferences)
            .SetValidator(new FoodPreferenceValidator());
    }
}