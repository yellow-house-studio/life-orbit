using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.AddFamilyMember;
using YellowHouseStudio.LifeOrbit.Tests.API.Infrastructure;

namespace YellowHouseStudio.LifeOrbit.Tests.API.Family;

public class AddFamilyMemberTests : ApiTestBase
{
    [Test]
    public async Task AddFamilyMember_with_valid_command_returns_new_member_id()
    {
        // Arrange
        var command = new AddFamilyMemberCommand
        {
            UserId = Guid.NewGuid(),
            Name = "Test Member",
            Age = 25
        };

        // Act
        var response = await Client.PostAsJsonAsync("settings/family", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<Guid>();
        result.Should().NotBe(Guid.Empty);
    }

    [Test]
    public async Task AddFamilyMember_with_invalid_age_returns_bad_request()
    {
        // Arrange
        var command = new AddFamilyMemberCommand
        {
            UserId = Guid.NewGuid(),
            Name = "Test Member",
            Age = -1 // Invalid age
        };

        // Act
        var response = await Client.PostAsJsonAsync("settings/family", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        problemDetails.Title.Should().Be("One or more validation errors occurred.");
        problemDetails.Errors.Should().ContainKey("Age")
            .WhoseValue.Should().Contain("Age must be greater than or equal to 0");
    }
}