using NUnit.Framework;
using FluentAssertions;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveSafeFood;

namespace YellowHouseStudio.LifeOrbit.Tests.Unit.Application.Family.RemoveSafeFood;

[TestFixture]
public class RemoveSafeFoodCommandValidatorTests
{
    private RemoveSafeFoodCommandValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new RemoveSafeFoodCommandValidator();
    }

    [Test]
    public void Validate_should_fail_when_FamilyMemberId_is_empty()
    {
        var command = new RemoveSafeFoodCommand { FamilyMemberId = Guid.Empty, FoodItem = "Apple" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FamilyMemberId");
    }

    [Test]
    public void Validate_should_fail_when_FoodItem_is_empty()
    {
        var command = new RemoveSafeFoodCommand { FamilyMemberId = Guid.NewGuid(), FoodItem = string.Empty };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FoodItem");
    }

    [Test]
    public void Validate_should_fail_when_FoodItem_exceeds_max_length()
    {
        var command = new RemoveSafeFoodCommand { FamilyMemberId = Guid.NewGuid(), FoodItem = new string('A', 101) };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FoodItem");
    }

    [Test]
    public void Validate_should_succeed_when_command_is_valid()
    {
        var command = new RemoveSafeFoodCommand { FamilyMemberId = Guid.NewGuid(), FoodItem = "Apple" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}