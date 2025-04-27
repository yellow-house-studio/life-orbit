using NUnit.Framework;
using FluentAssertions;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Tests.Unit.Domain.Family;

[TestFixture]
public class SafeFoodTests
{
    [Test]
    public void Constructor_should_set_food_item_and_generate_id()
    {
        var foodItem = "Apple";
        var safeFood = new SafeFood(foodItem);
        safeFood.FoodItem.Should().Be(foodItem);
        safeFood.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Two_safe_foods_with_same_food_item_should_not_be_equal_by_reference()
    {
        var a = new SafeFood("Apple");
        var b = new SafeFood("Apple");
        a.Should().NotBeSameAs(b);
        a.FoodItem.Should().Be(b.FoodItem);
    }
} 