using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.AddFoodPreference;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using FluentAssertions;

namespace YellowHouseStudio.LifeOrbit.Tests.Unit.Application.Family.AddFoodPreference;

[TestFixture]
public class AddFoodPreferenceCommandValidatorTests
{
    private AddFoodPreferenceCommandValidator _validator = null!;
    private Mock<IFamilyMemberRepository> _repository = null!;
    private Mock<ICurrentUser> _currentUser = null!;
    private Guid _userId;

    [SetUp]
    public void Setup()
    {
        _userId = Guid.NewGuid();
        _repository = new Mock<IFamilyMemberRepository>();
        _currentUser = new Mock<ICurrentUser>();
        _currentUser.Setup(x => x.UserId).Returns(_userId);
        _validator = new AddFoodPreferenceCommandValidator(_repository.Object, _currentUser.Object);
    }

    [Test]
    public async Task Should_have_error_when_FamilyMemberId_is_empty()
    {
        var command = new AddFoodPreferenceCommand { FamilyMemberId = Guid.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FamilyMemberId);
    }

    [Test]
    public async Task Should_have_error_when_FoodItem_is_empty()
    {
        var command = new AddFoodPreferenceCommand { FoodItem = string.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FoodItem);
    }

    [Test]
    public async Task Should_have_error_when_FoodItem_is_too_long()
    {
        var command = new AddFoodPreferenceCommand { FoodItem = new string('a', 101) };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FoodItem);
    }

    [Test]
    public async Task Should_have_error_when_Status_is_empty()
    {
        var command = new AddFoodPreferenceCommand { Status = string.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Test]
    public async Task Should_have_error_when_Status_is_invalid()
    {
        var command = new AddFoodPreferenceCommand { Status = "InvalidStatus" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Test]
    public async Task Should_have_error_when_family_member_not_found()
    {
        var command = new AddFoodPreferenceCommand { FamilyMemberId = Guid.NewGuid() };
        _repository.Setup(x => x.ExistsAsync(command.FamilyMemberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var result = await _validator.TestValidateAsync(command);
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Family member not found"));
    }

    [Test]
    public async Task Should_have_error_when_food_preference_already_exists()
    {
        var command = new AddFoodPreferenceCommand { FamilyMemberId = Guid.NewGuid(), FoodItem = "Apple" };
        _repository.Setup(x => x.ExistsAsync(command.FamilyMemberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repository.Setup(x => x.HasFoodPreferenceAsync(command.FamilyMemberId, command.FoodItem, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var result = await _validator.TestValidateAsync(command);
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Food preference already exists"));
    }

    [Test]
    public async Task Should_not_have_error_when_command_is_valid()
    {
        var command = new AddFoodPreferenceCommand
        {
            FamilyMemberId = Guid.NewGuid(),
            FoodItem = "Apple",
            Status = PreferenceStatus.Include.ToString()
        };
        _repository.Setup(x => x.ExistsAsync(command.FamilyMemberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repository.Setup(x => x.HasFoodPreferenceAsync(command.FamilyMemberId, command.FoodItem, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}