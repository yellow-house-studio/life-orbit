using FluentValidation;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddFoodPreference;

public class AddFoodPreferenceCommandValidator : AbstractValidator<AddFoodPreferenceCommand>
{
    public AddFoodPreferenceCommandValidator(IFamilyMemberRepository repository, ICurrentUser currentUser)
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

        RuleFor(x => x)
            .MustAsync(async (cmd, _, ct) =>
                await repository.ExistsAsync(cmd.FamilyMemberId, currentUser.UserId, ct))
            .WithMessage("Family member not found or not owned by user.")
            .WithErrorCode(ValidationErrorCodes.NotFound);

        RuleFor(x => x)
            .MustAsync(async (cmd, _, ct) =>
                !await repository.HasFoodPreferenceAsync(cmd.FamilyMemberId, cmd.FoodItem, ct))
            .WithMessage("Food preference already exists for this family member.")
            .WithErrorCode(ValidationErrorCodes.BadRequest);
    }
}