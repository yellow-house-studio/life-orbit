using YellowHouseStudio.LifeOrbit.Application.Common.Commands;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Application.Family.UpdateFoodPreference;

public record UpdateFoodPreferenceCommand : ICommand<FamilyMemberResponse>
{
    public Guid FamilyMemberId { get; init; }
    public string FoodItem { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}