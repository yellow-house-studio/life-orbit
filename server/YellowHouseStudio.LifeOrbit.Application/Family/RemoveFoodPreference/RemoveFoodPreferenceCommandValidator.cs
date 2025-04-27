using FluentValidation;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;

namespace YellowHouseStudio.LifeOrbit.Application.Family.RemoveFoodPreference;

public class RemoveFoodPreferenceCommandValidator : AbstractValidator<RemoveFoodPreferenceCommand>
{
    public RemoveFoodPreferenceCommandValidator(IFamilyMemberRepository repository, ICurrentUser currentUser)
    {
        RuleFor(x => x.FamilyMemberId)
            .NotEmpty();

        RuleFor(x => x.FoodItem)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x)
            .MustAsync(async (cmd, _, ct) =>
                await repository.ExistsAsync(cmd.FamilyMemberId, currentUser.UserId, ct))
            .WithMessage("Family member not found or not owned by user.")
            .WithErrorCode(ValidationErrorCodes.NotFound);

        RuleFor(x => x)
            .MustAsync(async (cmd, _, ct) =>
                await repository.HasFoodPreferenceAsync(cmd.FamilyMemberId, cmd.FoodItem, ct))
            .WithMessage("Food preference not found for this family member.")
            .WithErrorCode(ValidationErrorCodes.NotFound);
    }
}