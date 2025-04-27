namespace YellowHouseStudio.LifeOrbit.Domain.Family;

/// <summary>
/// Represents a food preference for a family member.
/// </summary>
public class FoodPreference
{
    /// <summary>
    /// Gets the unique identifier for the food preference.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the name of the food item.
    /// </summary>
    public string FoodItem { get; private set; }

    /// <summary>
    /// Gets the preference status for the food item.
    /// </summary>
    public PreferenceStatus Status { get; private set; }

    private FoodPreference()
    {
        // Initialize non-nullable properties
        FoodItem = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the FoodPreference class.
    /// </summary>
    /// <param name="foodItem">The name of the food item.</param>
    /// <param name="status">The preference status for the food item.</param>
    public FoodPreference(string foodItem, PreferenceStatus status)
    {
        Id = Guid.NewGuid();
        FoodItem = foodItem;
        Status = status;
    }

    /// <summary>
    /// Updates the status of the food preference.
    /// </summary>
    /// <param name="status">The new preference status.</param>
    public void UpdateStatus(PreferenceStatus status)
    {
        Status = status;
    }
}

/// <summary>
/// Defines the status levels for food preferences.
/// </summary>
public enum PreferenceStatus
{
    /// <summary>
    /// The food item should be included in meals.
    /// </summary>
    Include,

    /// <summary>
    /// The food item can be present in meals for other family members.
    /// </summary>
    AvailableForOthers,

    /// <summary>
    /// The food item should not be included in any meals.
    /// </summary>
    NotAllowed
}