using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.AddFamilyMember;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration.Application.Family;

[TestFixture]
public class AddFamilyMemberCommandHandlerTests : TestBase
{
    [Test]
    public async Task Handle_adds_family_member_and_returns_id()
    {
        // Arrange
        var command = new AddFamilyMemberCommand
        {
            UserId = Guid.NewGuid(),
            Name = "John Doe",
            Age = 30
        };

        // Act
        var result = await Mediator.Send(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();

        var familyMember = await Context.FamilyMembers
            .FirstOrDefaultAsync(fm => fm.Id == result);

        familyMember.Should().NotBeNull();
        familyMember!.UserId.Should().Be(command.UserId);
        familyMember.Name.Should().Be(command.Name);
        familyMember.Age.Should().Be(command.Age);
        familyMember.Allergies.Should().BeEmpty();
        familyMember.SafeFoods.Should().BeEmpty();
        familyMember.FoodPreferences.Should().BeEmpty();
    }

    [Test]
    public async Task Handle_creates_unique_family_members_for_same_user()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command1 = new AddFamilyMemberCommand
        {
            UserId = userId,
            Name = "John Doe",
            Age = 30
        };
        var command2 = new AddFamilyMemberCommand
        {
            UserId = userId,
            Name = "Jane Doe",
            Age = 28
        };

        // Act
        var result1 = await Mediator.Send(command1, CancellationToken.None);
        var result2 = await Mediator.Send(command2, CancellationToken.None);

        // Assert
        result1.Should().NotBe(result2);

        var familyMembers = await Context.FamilyMembers
            .Where(fm => fm.UserId == userId)
            .ToListAsync();

        familyMembers.Should().HaveCount(2);
        familyMembers.Should().Contain(fm => fm.Name == "John Doe" && fm.Age == 30);
        familyMembers.Should().Contain(fm => fm.Name == "Jane Doe" && fm.Age == 28);
    }
} 