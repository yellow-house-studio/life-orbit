using YellowHouseStudio.LifeOrbit.Application.Common.Commands;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddAllergy;

/// <summary>
/// Command to add an allergy to a family member
/// </summary>
public record AddAllergyCommand : ICommand<FamilyMemberResponse>
{
    /// <summary>
    /// The ID of the family member to add the allergy to
    /// </summary>
    public Guid FamilyMemberId { get; init; }

    /// <summary>
    /// The allergen to add (e.g., "Peanuts", "Milk", etc.)
    /// </summary>
    public string Allergen { get; init; } = string.Empty;

    /// <summary>
    /// The severity of the allergy (must be a valid AllergySeverity value)
    /// </summary>
    public string Severity { get; init; } = string.Empty;
}