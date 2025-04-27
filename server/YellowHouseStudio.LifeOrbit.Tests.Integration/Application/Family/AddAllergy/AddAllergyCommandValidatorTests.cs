using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.AddAllergy;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using YellowHouseStudio.LifeOrbit.Infrastructure.Repositories;
using YellowHouseStudio.LifeOrbit.Tests.Common.Builders;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration.Application.Family.AddAllergy;

[TestFixture]
[Category("Integration")]
public class AddAllergyCommandValidatorTests : TestBase
{
    private AddAllergyCommandValidator _validator = null!;
    private Guid _familyMemberId;
    private FamilyMember _familyMember = null!;
    private IFamilyMemberRepository _repository = null!;
    [SetUp]
    public async Task Setup()
    {
        _repository = new FamilyMemberRepository(Context);

        _validator = new AddAllergyCommandValidator(_repository, CurrentUser);

        // Create a family member to test with
        _familyMember = new FamilyMemberBuilder()
            .WithUserId(CurrentUser.UserId)
            .WithName("Test Member")
            .WithAllergy("Existing Allergy", AllergySeverity.NotAllowed)
            .Build();
            
        Context.FamilyMembers.Add(_familyMember);
        await Context.SaveChangesAsync();
        _familyMemberId = _familyMember.Id;
    }

    [Test]
    [Category("Validation")]
    public async Task Should_Have_Error_When_FamilyMemberId_Empty()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(Guid.Empty)
            .Build();

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FamilyMemberId)
            .WithErrorMessage("Please select a family member");
    }

    [Test]
    [Category("Validation")]
    public async Task Should_Have_Error_When_FamilyMember_Not_Found()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(Guid.NewGuid())
            .Build();

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FamilyMemberId)
            .WithErrorMessage("Family member not found");
    }

    [Test]
    [Category("Validation")]
    public async Task Should_Have_Error_When_Allergen_Empty()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen(string.Empty)
            .Build();

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Allergen)
            .WithErrorMessage("Please enter an allergen");
    }

    [Test]
    [Category("Validation")]
    public async Task Should_Have_Error_When_Allergen_Too_Long()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen(new string('x', 101))
            .Build();

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Allergen)
            .WithErrorMessage("Allergen name cannot be longer than 100 characters");
    }

    [Test]
    [Category("Validation")]
    public async Task Should_Have_Error_When_Allergy_Already_Exists()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen("Existing Allergy")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Allergen)
            .WithErrorMessage("This allergy is already registered for this family member");
    }

    [Test]
    [Category("Validation")]
    public async Task Should_Not_Have_Error_When_Command_Valid()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen("Peanuts")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
} 