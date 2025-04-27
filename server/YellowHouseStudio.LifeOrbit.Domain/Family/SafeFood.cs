using System;

namespace YellowHouseStudio.LifeOrbit.Domain.Family;

/// <summary>
/// Represents a safe food item for a family member.
/// </summary>
public class SafeFood
{
    /// <summary>
    /// Gets the unique identifier for the safe food.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the name of the food item.
    /// </summary>
    public string FoodItem { get; private set; }

    private SafeFood()
    {
        // Initialize non-nullable properties
        FoodItem = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the SafeFood class.
    /// </summary>
    /// <param name="foodItem">The name of the food item.</param>
    public SafeFood(string foodItem)
    {
        Id = Guid.NewGuid();
        FoodItem = foodItem;
    }
}