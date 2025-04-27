using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.AddCompleteFamilyMember;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration.Application.Family;

[TestFixture]
public class AddCompleteFamilyMemberCommandHandlerTests : TestBase
{
    [Test]
    public async Task Handle_adds_complete_family_member_with_all_details()
    {
        // Arrange
        var command = new AddCompleteFamilyMemberCommand
        {
            Name = "Test User",
            Age = 30,
            Allergies = new List<AllergyDto>
            {
                new() { Allergen = "Peanuts", Severity = "NotAllowed" }
            },
            SafeFoods = new List<SafeFoodDto>
            {
                new() { FoodItem = "Apple" }
            },
            FoodPreferences = new List<FoodPreferenceDto>
            {
                new() { FoodItem = "Spinach", Status = "Include" }
            }
        };

        // Act
        var result = await Mediator.Send(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Age.Should().Be(command.Age);

        var savedMember = await Context.FamilyMembers
            .Include(fm => fm.Allergies)
            .Include(fm => fm.SafeFoods)
            .Include(fm => fm.FoodPreferences)
            .FirstOrDefaultAsync(fm => fm.Id == result.Id);

        savedMember.Should().NotBeNull();
        savedMember!.UserId.Should().Be(CurrentUser.UserId);
        savedMember.Name.Should().Be(command.Name);
        savedMember.Age.Should().Be(command.Age);

        savedMember.Allergies.Should().HaveCount(1);
        savedMember.Allergies[0].Allergen.Should().Be("Peanuts");
        savedMember.Allergies[0].Severity.Should().Be(AllergySeverity.NotAllowed);

        savedMember.SafeFoods.Should().HaveCount(1);
        savedMember.SafeFoods[0].FoodItem.Should().Be("Apple");

        savedMember.FoodPreferences.Should().HaveCount(1);
        savedMember.FoodPreferences[0].FoodItem.Should().Be("Spinach");
        savedMember.FoodPreferences[0].Status.Should().Be(PreferenceStatus.Include);
    }

    [Test]
    public async Task Handle_creates_unique_complete_family_members_for_same_user()
    {
        // Arrange
        var command1 = new AddCompleteFamilyMemberCommand
        {
            Name = "John Doe",
            Age = 30,
            Allergies = new List<AllergyDto>
            {
                new() { Allergen = "Peanuts", Severity = "NotAllowed" }
            }
        };
        var command2 = new AddCompleteFamilyMemberCommand
        {
            Name = "Jane Doe",
            Age = 28,
            SafeFoods = new List<SafeFoodDto>
            {
                new() { FoodItem = "Apple" }
            }
        };

        // Act
        var result1 = await Mediator.Send(command1, CancellationToken.None);
        var result2 = await Mediator.Send(command2, CancellationToken.None);

        // Assert
        result1.Id.Should().NotBe(result2.Id);

        var familyMembers = await Context.FamilyMembers
            .Include(fm => fm.Allergies)
            .Include(fm => fm.SafeFoods)
            .Where(fm => fm.UserId == CurrentUser.UserId)
            .ToListAsync();

        familyMembers.Should().HaveCount(2);

        var member1 = familyMembers.First(fm => fm.Id == result1.Id);
        member1.Name.Should().Be("John Doe");
        member1.Age.Should().Be(30);
        member1.Allergies.Should().HaveCount(1);
        member1.Allergies[0].Allergen.Should().Be("Peanuts");
        member1.SafeFoods.Should().BeEmpty();

        var member2 = familyMembers.First(fm => fm.Id == result2.Id);
        member2.Name.Should().Be("Jane Doe");
        member2.Age.Should().Be(28);
        member2.Allergies.Should().BeEmpty();
        member2.SafeFoods.Should().HaveCount(1);
        member2.SafeFoods[0].FoodItem.Should().Be("Apple");
    }
}