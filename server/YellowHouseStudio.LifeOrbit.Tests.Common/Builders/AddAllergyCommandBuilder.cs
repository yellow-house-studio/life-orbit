using YellowHouseStudio.LifeOrbit.Application.Family.AddAllergy;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Tests.Common.Builders;

/// <summary>
/// Builder for creating AddAllergyCommand instances for testing
/// </summary>
public class AddAllergyCommandBuilder
{
    private Guid _familyMemberId = Guid.NewGuid();
    private string _allergen = "Peanuts";
    private string _severity = AllergySeverity.NotAllowed.ToString();

    /// <summary>
    /// Sets the family member ID for the command
    /// </summary>
    public AddAllergyCommandBuilder WithFamilyMemberId(Guid familyMemberId)
    {
        _familyMemberId = familyMemberId;
        return this;
    }

    /// <summary>
    /// Sets the allergen name
    /// </summary>
    public AddAllergyCommandBuilder WithAllergen(string allergen)
    {
        _allergen = allergen;
        return this;
    }

    /// <summary>
    /// Sets the allergy severity
    /// </summary>
    public AddAllergyCommandBuilder WithSeverity(AllergySeverity severity)
    {
        _severity = severity.ToString();
        return this;
    }

    /// <summary>
    /// Sets the allergy severity as a string
    /// </summary>
    public AddAllergyCommandBuilder WithSeverityString(string severity)
    {
        _severity = severity;
        return this;
    }

    /// <summary>
    /// Builds the AddAllergyCommand instance
    /// </summary>
    public AddAllergyCommand Build()
    {
        return new AddAllergyCommand
        {
            FamilyMemberId = _familyMemberId,
            Allergen = _allergen,
            Severity = _severity
        };
    }
} 