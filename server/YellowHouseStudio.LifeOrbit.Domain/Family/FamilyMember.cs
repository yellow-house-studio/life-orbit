namespace YellowHouseStudio.LifeOrbit.Domain.Family;

/// <summary>
/// Represents a family member in the system with their food-related preferences and restrictions.
/// </summary>
public class FamilyMember
{
    /// <summary>
    /// Gets the unique identifier for the family member.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the user who owns this family member record.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the name of the family member.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the age of the family member.
    /// </summary>
    public int Age { get; private set; }

    /// <summary>
    /// Gets the list of allergies associated with this family member.
    /// </summary>
    public List<Allergy> Allergies { get; private set; } = new();

    /// <summary>
    /// Gets the list of safe foods for this family member (particularly relevant for selective eaters).
    /// </summary>
    public List<SafeFood> SafeFoods { get; private set; } = new();

    /// <summary>
    /// Gets the list of food preferences for this family member.
    /// </summary>
    public List<FoodPreference> FoodPreferences { get; private set; } = new();

    private FamilyMember()
    {
        // Initialize non-nullable properties
        Name = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the FamilyMember class.
    /// </summary>
    /// <param name="userId">The ID of the user who owns this family member record.</param>
    /// <param name="name">The name of the family member.</param>
    /// <param name="age">The age of the family member.</param>
    public FamilyMember(Guid userId, string name, int age)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;
        Age = age;
    }

    /// <summary>
    /// Updates the basic details of the family member.
    /// </summary>
    /// <param name="name">The new name of the family member.</param>
    /// <param name="age">The new age of the family member.</param>
    public void UpdateDetails(string name, int age)
    {
        Name = name;
        Age = age;
    }

    /// <summary>
    /// Adds or updates an allergy for the family member.
    /// </summary>
    /// <param name="allergen">The allergen to add or update.</param>
    /// <param name="severity">The severity level of the allergy.</param>
    public void AddAllergy(string allergen, AllergySeverity severity)
    {
        var existing = Allergies.Find(a => a.Allergen.Equals(allergen, StringComparison.OrdinalIgnoreCase));
        if (existing != null)
        {
            existing.UpdateSeverity(severity);
        }
        else
        {
            Allergies.Add(new Allergy(allergen, severity));
        }
    }

    /// <summary>
    /// Removes an allergy from the family member's list of allergies.
    /// </summary>
    /// <param name="allergen">The allergen to remove.</param>
    public void RemoveAllergy(string allergen)
    {
        Allergies.RemoveAll(a => a.Allergen == allergen);
    }

    /// <summary>
    /// Adds a safe food item for the family member.
    /// </summary>
    /// <param name="foodItem">The food item to add as safe.</param>
    public void AddSafeFood(string foodItem)
    {
        if (!SafeFoods.Exists(sf => sf.FoodItem == foodItem))
        {
            SafeFoods.Add(new SafeFood(foodItem));
        }
    }

    /// <summary>
    /// Removes a safe food item from the family member's list of safe foods.
    /// </summary>
    /// <param name="foodItem">The food item to remove.</param>
    public void RemoveSafeFood(string foodItem)
    {
        SafeFoods.RemoveAll(sf => sf.FoodItem == foodItem);
    }

    /// <summary>
    /// Adds or updates a food preference for the family member.
    /// </summary>
    /// <param name="foodItem">The food item to set a preference for.</param>
    /// <param name="status">The preference status for the food item.</param>
    public void AddOrUpdatePreference(string foodItem, PreferenceStatus status)
    {
        var existing = FoodPreferences.Find(p => p.FoodItem == foodItem);
        if (existing != null)
        {
            existing.UpdateStatus(status);
        }
        else
        {
            FoodPreferences.Add(new FoodPreference(foodItem, status));
        }
    }

    /// <summary>
    /// Removes a food preference from the family member's list of preferences.
    /// </summary>
    /// <param name="foodItem">The food item to remove the preference for.</param>
    public void RemovePreference(string foodItem)
    {
        FoodPreferences.RemoveAll(p => p.FoodItem == foodItem);
    }
}