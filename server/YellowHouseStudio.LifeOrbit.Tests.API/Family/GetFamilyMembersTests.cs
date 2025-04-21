using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.GetFamilyMembers;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Tests.API.Infrastructure;

namespace YellowHouseStudio.LifeOrbit.Tests.API.Family;

public class GetFamilyMembersTests : ApiTestBase
{
    [Test]
    public async Task GetFamilyMembers_returns_empty_list_when_no_members()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"settings/family?userId={userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<FamilyMemberResponse>>();
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetFamilyMembers_WithInvalidUserId_ReturnsBadRequest()
    {
        // Act
        var response = await Client.GetAsync("settings/family?userId=invalid-guid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
} 