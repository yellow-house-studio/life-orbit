namespace YellowHouseStudio.LifeOrbit.Domain.Family;

/// <summary>
/// Represents an allergy for a family member.
/// </summary>
public class Allergy
{
    /// <summary>
    /// Gets the unique identifier for the allergy.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the name of the allergen.
    /// </summary>
    public string Allergen { get; private set; }

    /// <summary>
    /// Gets the severity level of the allergy.
    /// </summary>
    public AllergySeverity Severity { get; private set; }

    private Allergy()
    {
        // Initialize non-nullable properties
        Allergen = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the Allergy class.
    /// </summary>
    /// <param name="allergen">The name of the allergen.</param>
    /// <param name="severity">The severity level of the allergy.</param>
    public Allergy(string allergen, AllergySeverity severity)
    {
        Id = Guid.NewGuid();
        Allergen = allergen;
        Severity = severity;
    }

    /// <summary>
    /// Updates the severity level of the allergy.
    /// </summary>
    /// <param name="severity">The new severity level.</param>
    public void UpdateSeverity(AllergySeverity severity)
    {
        Severity = severity;
    }
}

/// <summary>
/// Defines the severity levels for allergies.
/// </summary>
public enum AllergySeverity
{
    /// <summary>
    /// The allergen can be present in meals for other family members.
    /// </summary>
    AvailableForOthers,

    /// <summary>
    /// The allergen is not allowed in any meals.
    /// </summary>
    NotAllowed
}