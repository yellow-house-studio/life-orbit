using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.AddCompleteFamilyMember;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Tests.API.Infrastructure;

namespace YellowHouseStudio.LifeOrbit.Tests.API.Family;

public class AddCompleteFamilyMemberTests : ApiTestBase
{
    [Test]
    public async Task AddCompleteFamilyMember_with_valid_command_returns_created_member()
    {
        // Arrange
        var command = new AddCompleteFamilyMemberCommand
        {
            Name = "Test Member",
            Age = 25,
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
        var response = await Client.PostAsJsonAsync("settings/family/complete", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<FamilyMemberResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().NotBe(Guid.Empty);
        result.Name.Should().Be(command.Name);
        result.Age.Should().Be(command.Age);
        result.Allergies.Should().HaveCount(1);
        result.SafeFoods.Should().HaveCount(1);
        result.FoodPreferences.Should().HaveCount(1);
    }

    [Test]
    public async Task AddCompleteFamilyMember_with_invalid_age_returns_bad_request()
    {
        // Arrange
        var command = new AddCompleteFamilyMemberCommand
        {
            Name = "Test Member",
            Age = -1 // Invalid age
        };

        // Act
        var response = await Client.PostAsJsonAsync("settings/family/complete", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Errors.Should().ContainKey("Age")
            .WhoseValue.Should().Contain("Age must be between 0 and 120");
    }

    [Test]
    public async Task AddCompleteFamilyMember_with_invalid_severity_returns_bad_request()
    {
        // Arrange
        var command = new AddCompleteFamilyMemberCommand
        {
            Name = "Test Member",
            Age = 25,
            Allergies = new List<AllergyDto>
            {
                new() { Allergen = "Peanuts", Severity = "Invalid" } // Invalid severity
            }
        };

        // Act
        var response = await Client.PostAsJsonAsync("settings/family/complete", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Errors.Should().ContainKey("Allergies[0].Severity")
            .WhoseValue.Should().Contain("Severity must be either 'AvailableForOthers' or 'NotAllowed'");
    }
} 