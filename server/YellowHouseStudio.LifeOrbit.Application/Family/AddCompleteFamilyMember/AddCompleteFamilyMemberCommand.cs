using YellowHouseStudio.LifeOrbit.Application.Common.Commands;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddCompleteFamilyMember;

public record AddCompleteFamilyMemberCommand : ICommand<FamilyMemberResponse>
{
    public string Name { get; init; } = string.Empty;

    public int Age { get; init; }

    public List<AllergyDto> Allergies { get; init; } = new();
    public List<SafeFoodDto> SafeFoods { get; init; } = new();
    public List<FoodPreferenceDto> FoodPreferences { get; init; } = new();
}