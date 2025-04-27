using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using YellowHouseStudio.LifeOrbit.Tests.Common.Builders;
using FluentValidation;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration.Application.Family.AddAllergy;

[TestFixture]
[Category("Integration")]
public class AddAllergyCommandHandlerTests : TestBase
{
    private Guid _familyMemberId;

    [SetUp]
    public async Task Setup()
    {
        await base.BaseSetup();

        // Create a family member to test with
        var familyMember = new FamilyMemberBuilder()
            .WithUserId(CurrentUser.UserId)
            .WithName("Test Member")
            .Build();

        Context.FamilyMembers.Add(familyMember);
        await Context.SaveChangesAsync();
        _familyMemberId = familyMember.Id;
    }

    [Test]
    [Category("Happy Path")]
    public async Task Handle_adds_allergy_to_family_member()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen("Peanuts")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();

        // Act
        var result = await Mediator.Send(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(_familyMemberId);

        // Reload family member from database
        var familyMember = await Context.FamilyMembers
            .Include(fm => fm.Allergies)
            .FirstAsync(fm => fm.Id == _familyMemberId);

        familyMember.Allergies.Should().HaveCount(1);
        var allergy = familyMember.Allergies[0];
        allergy.Allergen.Should().Be("Peanuts");
        allergy.Severity.Should().Be(AllergySeverity.NotAllowed);
    }

    [Test]
    [Category("Happy Path")]
    public async Task Handle_adds_multiple_allergies_to_family_member()
    {
        // Arrange
        var allergies = new[]
        {
            ("Peanuts", AllergySeverity.NotAllowed),
            ("Milk", AllergySeverity.AvailableForOthers),
            ("Eggs", AllergySeverity.NotAllowed)
        };

        // Act
        foreach (var (allergen, severity) in allergies)
        {
            var command = new AddAllergyCommandBuilder()
                .WithFamilyMemberId(_familyMemberId)
                .WithAllergen(allergen)
                .WithSeverity(severity)
                .Build();

            await Mediator.Send(command, CancellationToken.None);
        }

        // Assert
        var familyMember = await Context.FamilyMembers
            .Include(fm => fm.Allergies)
            .FirstAsync(fm => fm.Id == _familyMemberId);

        familyMember.Allergies.Should().HaveCount(3);
        foreach (var (allergen, severity) in allergies)
        {
            familyMember.Allergies.Should().Contain(a =>
                a.Allergen == allergen &&
                a.Severity == severity);
        }
    }

    [Test]
    [Category("Error Handling")]
    public async Task Handle_throws_validation_error_for_non_existent_family_member()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(Guid.NewGuid())
            .WithAllergen("Test Allergen")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();

        // Act & Assert
        await FluentActions.Invoking(() =>
            Mediator.Send(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>()
            .WithMessage("*Family member not found*");
    }

    [Test]
    [Category("Error Handling")]
    public async Task Handle_prevents_duplicate_allergen()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen("Peanuts")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();

        // Add first allergy
        await Mediator.Send(command, CancellationToken.None);

        // Act & Assert - Try to add same allergy again
        await FluentActions.Invoking(() =>
            Mediator.Send(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>()
            .WithMessage("*This allergy is already registered for this family member*");
    }

    [Test]
    [Category("Error Handling")]
    public async Task Handle_throws_validation_error_for_different_user_family_member()
    {
        // Arrange
        var otherUserMember = new FamilyMemberBuilder()
            .WithUserId(Guid.NewGuid()) // Different user
            .WithName("Other User")
            .Build();
        Context.FamilyMembers.Add(otherUserMember);
        await Context.SaveChangesAsync();

        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(otherUserMember.Id)
            .WithAllergen("Test Allergen")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();

        // Act & Assert
        await FluentActions.Invoking(() =>
            Mediator.Send(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>()
            .WithMessage("*Family member not found*");
    }
}