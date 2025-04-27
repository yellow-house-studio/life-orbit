using YellowHouseStudio.LifeOrbit.Application.Common.Commands;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Application.Family.UpdateAllergy;

public record UpdateAllergyCommand : ICommand<FamilyMemberResponse>
{
    public Guid FamilyMemberId { get; init; }
    public string Allergen { get; init; } = string.Empty;
    public string Severity { get; init; } = string.Empty;
} 