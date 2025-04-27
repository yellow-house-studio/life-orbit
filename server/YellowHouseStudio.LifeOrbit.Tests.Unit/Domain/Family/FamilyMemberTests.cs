using NUnit.Framework;
using FluentAssertions;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Tests.Unit.Domain.Family;

[TestFixture]
public class FamilyMemberTests
{
    private FamilyMember _member = null!;

    [SetUp]
    public void SetUp()
    {
        _member = new FamilyMember(Guid.NewGuid(), "Test", 10);
    }

    [Test]
    public void AddSafeFood_should_add_new_safe_food()
    {
        _member.AddSafeFood("Apple");
        _member.SafeFoods.Should().ContainSingle(sf => sf.FoodItem == "Apple");
    }

    [Test]
    public void AddSafeFood_should_not_add_duplicate_safe_food()
    {
        _member.AddSafeFood("Apple");
        _member.AddSafeFood("Apple");
        _member.SafeFoods.Count(sf => sf.FoodItem == "Apple").Should().Be(1);
    }

    [Test]
    public void RemoveSafeFood_should_remove_existing_safe_food()
    {
        _member.AddSafeFood("Apple");
        _member.RemoveSafeFood("Apple");
        _member.SafeFoods.Should().NotContain(sf => sf.FoodItem == "Apple");
    }

    [Test]
    public void RemoveSafeFood_should_do_nothing_if_food_not_present()
    {
        _member.AddSafeFood("Apple");
        _member.RemoveSafeFood("Banana");
        _member.SafeFoods.Should().ContainSingle(sf => sf.FoodItem == "Apple");
    }

    [Test]
    public void SafeFoods_should_be_empty_by_default()
    {
        _member.SafeFoods.Should().BeEmpty();
    }
} 