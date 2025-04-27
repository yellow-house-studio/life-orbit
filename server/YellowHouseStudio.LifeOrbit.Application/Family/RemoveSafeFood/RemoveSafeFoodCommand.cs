using YellowHouseStudio.LifeOrbit.Application.Common.Commands;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Application.Family.RemoveSafeFood;

public record RemoveSafeFoodCommand : ICommand<SafeFoodResult>
{
    public Guid FamilyMemberId { get; init; }
    public string FoodItem { get; init; } = string.Empty;
}