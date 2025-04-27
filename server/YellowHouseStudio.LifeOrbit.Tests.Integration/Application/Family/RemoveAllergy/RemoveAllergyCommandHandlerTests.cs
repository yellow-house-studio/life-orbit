// Integration tests for RemoveAllergyCommandHandler
// (Implement similar to AddAllergyCommandHandlerTests, using Arrange-Act-Assert) 

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveAllergy;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using YellowHouseStudio.LifeOrbit.Tests.Common.Builders;
using FluentValidation;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration.Application.Family.RemoveAllergy;

[TestFixture]
public class RemoveAllergyCommandHandlerTests : TestBase
{
    private Guid _familyMemberId;
    private string _allergen = "Peanuts";

    [SetUp]
    public async Task Setup()
    {
        await base.BaseSetup();
        var familyMember = new FamilyMemberBuilder()
            .WithUserId(CurrentUser.UserId)
            .WithName("Test Member")
            .WithAge(30)
            .Build();
        familyMember.AddAllergy(_allergen, AllergySeverity.NotAllowed);
        Context.FamilyMembers.Add(familyMember);
        await Context.SaveChangesAsync();
        _familyMemberId = familyMember.Id;
    }

    [Test]
    public async Task RemoveAllergy_removes_allergy_successfully()
    {
        // Arrange
        var command = new RemoveAllergyCommand { FamilyMemberId = _familyMemberId, Allergen = _allergen };

        // Act
        var result = await Mediator.Send(command, CancellationToken.None);

        // Assert
        result.Allergies.Should().BeEmpty();
        var member = await Context.FamilyMembers.Include(fm => fm.Allergies).FirstAsync(fm => fm.Id == _familyMemberId);
        member.Allergies.Should().BeEmpty();
    }

    [Test]
    public async Task RemoveAllergy_throws_validation_for_nonexistent_member()
    {
        // Arrange
        var command = new RemoveAllergyCommand { FamilyMemberId = Guid.NewGuid(), Allergen = _allergen };

        // Act & Assert
        await FluentActions.Invoking(() => Mediator.Send(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>()
            .WithMessage("*Family member not found*");
    }

    [Test]
    public async Task RemoveAllergy_throws_validation_for_nonexistent_allergy()
    {
        // Arrange
        var command = new RemoveAllergyCommand { FamilyMemberId = _familyMemberId, Allergen = "Nonexistent" };

        // Act & Assert
        await FluentActions.Invoking(() => Mediator.Send(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>()
            .WithMessage("*not registered*");
    }

    [Test]
    public async Task RemoveAllergy_throws_validation_for_unauthorized_user()
    {
        // Arrange
        var otherUserId = Guid.NewGuid();
        var familyMember = new FamilyMemberBuilder()
            .WithUserId(otherUserId)
            .WithName("Other Member")
            .WithAge(25)
            .Build();
        familyMember.AddAllergy(_allergen, AllergySeverity.NotAllowed);
        Context.FamilyMembers.Add(familyMember);
        await Context.SaveChangesAsync();

        var command = new RemoveAllergyCommand { FamilyMemberId = familyMember.Id, Allergen = _allergen };

        // Act & Assert
        await FluentActions.Invoking(() => Mediator.Send(command, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>()
            .WithMessage("*Family member not found*");
    }
}