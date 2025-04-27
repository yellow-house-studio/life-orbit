using YellowHouseStudio.LifeOrbit.Application.Common.Commands;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Application.Family.RemoveAllergy;

public record RemoveAllergyCommand : ICommand<FamilyMemberResponse>
{
    public Guid FamilyMemberId { get; init; }
    public string Allergen { get; init; } = string.Empty;
}