using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using YellowHouseStudio.LifeOrbit.Tests.Common.Builders;

namespace YellowHouseStudio.LifeOrbit.Tests.API.Controllers;

[TestFixture]
[Category("API")]
public class FamilyAllergiesControllerTests : ApiTestBase
{
    private Guid _familyMemberId;

    [SetUp]
    public override async Task Setup()
    {
        await base.Setup();

        // Create a family member to test with
        var familyMember = new FamilyMemberBuilder()
            .WithUserId(CurrentUser.UserId)
            .WithName("Test Member")
            .WithAge(30)
            .Build();

        Context.FamilyMembers.Add(familyMember);
        await Context.SaveChangesAsync();
        _familyMemberId = familyMember.Id;
    }

    [Test]
    [Category("Happy Path")]
    public async Task AddAllergy_adds_allergy_to_family_member()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen("Peanuts")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();

        // Act
        var response = await Client.PostAsJsonAsync($"settings/family/{_familyMemberId}/allergies", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<FamilyMemberResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(_familyMemberId);
        result.Allergies.Should().ContainSingle(a =>
            a.Allergen == "Peanuts" &&
            a.Severity == AllergySeverity.NotAllowed.ToString());

        // Verify database state
        var familyMember = await Context.FamilyMembers
            .Include(fm => fm.Allergies)
            .FirstAsync(fm => fm.Id == _familyMemberId);

        familyMember.Allergies.Should().HaveCount(1);
        var allergy = familyMember.Allergies[0];
        allergy.Allergen.Should().Be("Peanuts");
        allergy.Severity.Should().Be(AllergySeverity.NotAllowed);
    }

    [Test]
    [Category("Error Handling")]
    public async Task AddAllergy_returns_bad_request_when_ids_dont_match()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(Guid.NewGuid()) // Different from route ID
            .WithAllergen("Peanuts")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();

        // Act
        var response = await Client.PostAsJsonAsync($"settings/family/{_familyMemberId}/allergies", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Family member ID in route must match ID in command");
    }

    [Test]
    [Category("Error Handling")]
    public async Task AddAllergy_returns_not_found_for_non_existent_member()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(nonExistentId)
            .WithAllergen("Peanuts")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();

        // Act
        var response = await Client.PostAsJsonAsync($"settings/family/{nonExistentId}/allergies", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain($"Family member not found");
    }

    [Test]
    [Category("Error Handling")]
    public async Task AddAllergy_returns_bad_request_for_empty_allergen()
    {
        // Arrange
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen(string.Empty)
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();

        // Act
        var response = await Client.PostAsJsonAsync($"settings/family/{_familyMemberId}/allergies", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Please enter an allergen");
    }

    [Test]
    [Category("Error Handling")]
    public async Task AddAllergy_returns_bad_request_for_duplicate_allergy()
    {
        // Arrange - Add first allergy
        var command = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen("Peanuts")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();
        await Client.PostAsJsonAsync($"settings/family/{_familyMemberId}/allergies", command);

        // Act - Try to add same allergy again
        var response = await Client.PostAsJsonAsync($"settings/family/{_familyMemberId}/allergies", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("This allergy is already registered for this family member");
    }

    [Test]
    [Category("Happy Path")]
    public async Task RemoveAllergy_removes_allergy_from_family_member()
    {
        // Arrange - Add an allergy first
        var addCommand = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen("Peanuts")
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();
        await Client.PostAsJsonAsync($"settings/family/{_familyMemberId}/allergies", addCommand);

        // Act
        var response = await Client.DeleteAsync($"settings/family/{_familyMemberId}/allergies/Peanuts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<FamilyMemberResponse>();
        result.Should().NotBeNull();
        result!.Allergies.Should().BeEmpty();

        // Verify database state
        var familyMember = await Context.FamilyMembers
            .Include(fm => fm.Allergies)
            .FirstAsync(fm => fm.Id == _familyMemberId);

        familyMember.Allergies.Should().BeEmpty();
    }

    [Test]
    [Category("Error Handling")]
    public async Task RemoveAllergy_returns_not_found_for_non_existent_member()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"settings/family/{nonExistentId}/allergies/Peanuts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Family member not found");
    }

    [Test]
    [Category("Error Handling")]
    public async Task RemoveAllergy_returns_not_found_for_non_existent_allergy()
    {
        // Act
        var response = await Client.DeleteAsync($"settings/family/{_familyMemberId}/allergies/NonExistentAllergy");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("This allergy is not registered for this family member");
    }

    [Test]
    [Category("Happy Path")]
    public async Task RemoveAllergy_handles_url_encoded_allergen_names()
    {
        // Arrange - Add an allergy with special characters
        var allergen = "Tree Nuts & Seeds";
        var addCommand = new AddAllergyCommandBuilder()
            .WithFamilyMemberId(_familyMemberId)
            .WithAllergen(allergen)
            .WithSeverity(AllergySeverity.NotAllowed)
            .Build();
        await Client.PostAsJsonAsync($"settings/family/{_familyMemberId}/allergies", addCommand);

        // Act
        var response = await Client.DeleteAsync($"settings/family/{_familyMemberId}/allergies/{Uri.EscapeDataString(allergen)}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<FamilyMemberResponse>();
        result.Should().NotBeNull();
        result!.Allergies.Should().BeEmpty();
    }
}