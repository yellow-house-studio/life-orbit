using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.AddAllergy;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Tests.Unit.Application.Family.AddAllergy;

[TestFixture]
public class AddAllergyCommandValidatorTests
{
    private AddAllergyCommandValidator _validator = null!;
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
        _validator = new AddAllergyCommandValidator(_repository.Object, _currentUser.Object);
    }

    [Test]
    public async Task Should_have_error_when_FamilyMemberId_is_empty()
    {
        var command = new AddAllergyCommand { FamilyMemberId = Guid.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FamilyMemberId)
            .WithErrorMessage("Please select a family member");
    }

    [Test]
    public async Task Should_have_error_when_family_member_not_found()
    {
        // Arrange
        var command = new AddAllergyCommand { FamilyMemberId = Guid.NewGuid() };
        _repository.Setup(x => x.ExistsAsync(command.FamilyMemberId, _userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FamilyMemberId)
            .WithErrorMessage("Family member not found");
    }

    [Test]
    public async Task Should_have_error_when_Allergen_is_empty()
    {
        var command = new AddAllergyCommand { Allergen = string.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Allergen)
            .WithErrorMessage("Please enter an allergen");
    }

    [Test]
    public async Task Should_have_error_when_Allergen_is_too_long()
    {
        var command = new AddAllergyCommand { Allergen = new string('a', 101) };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Allergen)
            .WithErrorMessage("Allergen name cannot be longer than 100 characters");
    }

    [Test]
    public async Task Should_have_error_when_allergy_already_exists()
    {
        // Arrange
        var command = new AddAllergyCommand 
        { 
            FamilyMemberId = Guid.NewGuid(),
            Allergen = "Peanuts"
        };
        _repository.Setup(x => x.HasAllergyAsync(command.FamilyMemberId, command.Allergen, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Allergen)
            .WithErrorMessage("This allergy is already registered for this family member");
    }

    [Test]
    public async Task Should_have_error_when_Severity_is_empty()
    {
        var command = new AddAllergyCommand { Severity = string.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Severity)
            .WithErrorMessage("Please select an allergy severity");
    }

    [Test]
    public async Task Should_have_error_when_Severity_is_invalid()
    {
        var command = new AddAllergyCommand { Severity = "InvalidSeverity" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Severity)
            .WithErrorMessage("Please select a valid allergy severity level");
    }

    [Test]
    public async Task Should_not_have_error_when_command_is_valid()
    {
        // Arrange
        var command = new AddAllergyCommand
        {
            FamilyMemberId = Guid.NewGuid(),
            Allergen = "Peanuts",
            Severity = AllergySeverity.NotAllowed.ToString()
        };
        _repository.Setup(x => x.ExistsAsync(command.FamilyMemberId, _userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repository.Setup(x => x.HasAllergyAsync(command.FamilyMemberId, command.Allergen, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
} 