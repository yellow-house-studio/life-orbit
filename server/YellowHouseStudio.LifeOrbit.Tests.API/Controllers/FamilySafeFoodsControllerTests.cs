using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Tests.API.Controllers;

[TestFixture]
public class FamilySafeFoodsControllerTests : ApiTestBase
{
    private Guid _userId;
    private Guid _memberId;

    [SetUp]
    public async Task SetUp()
    {
        _userId = CurrentUser.UserId;
        // Create a family member for testing
        var member = new { Name = "Test", Age = 10, UserId = _userId };
        var response = await Client.PostAsJsonAsync("settings/family", member);
        response.EnsureSuccessStatusCode();
        var idString = await response.Content.ReadAsStringAsync();
        _memberId = Guid.Parse(idString.Trim('"'));
    }

    [Test]
    public async Task AddSafeFood_returns_200_and_result()
    {
        var request = new AddSafeFoodRequest { FoodItem = "Apple" };
        var response = await Client.PutAsJsonAsync($"settings/family/{_memberId}/safe-foods", request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SafeFoodResult>();
        result!.FamilyMemberId.Should().Be(_memberId);
        result.SafeFoods.Should().Contain("Apple");
    }

    [Test]
    public async Task AddSafeFood_returns_404_for_missing_member()
    {
        var request = new AddSafeFoodRequest { FoodItem = "Banana" };
        var response = await Client.PutAsJsonAsync($"settings/family/{Guid.NewGuid()}/safe-foods", request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task AddSafeFood_returns_400_for_invalid_request()
    {
        var request = new AddSafeFoodRequest { FoodItem = "" };
        var response = await Client.PutAsJsonAsync($"settings/family/{_memberId}/safe-foods", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task RemoveSafeFood_returns_200_and_result()
    {
        // Add first
        var addRequest = new AddSafeFoodRequest { FoodItem = "Orange" };
        var addResponse = await Client.PutAsJsonAsync($"settings/family/{_memberId}/safe-foods", addRequest);
        addResponse.EnsureSuccessStatusCode();
        // Remove
        var response = await Client.DeleteAsync($"settings/family/{_memberId}/safe-foods/Orange");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SafeFoodResult>();
        result!.FamilyMemberId.Should().Be(_memberId);
        result.SafeFoods.Should().NotContain("Orange");
    }

    [Test]
    public async Task RemoveSafeFood_returns_404_for_missing_member()
    {
        var response = await Client.DeleteAsync($"settings/family/{Guid.NewGuid()}/safe-foods/Apple");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task RemoveSafeFood_returns_404_for_missing_food()
    {
        var response = await Client.DeleteAsync($"settings/family/{_memberId}/safe-foods/NotPresent");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
} 