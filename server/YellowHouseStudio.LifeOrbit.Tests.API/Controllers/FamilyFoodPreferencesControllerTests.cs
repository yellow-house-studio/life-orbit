using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.AddFoodPreference;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Tests.API.Controllers;

[TestFixture]
public class FamilyFoodPreferencesControllerTests : ApiTestBase
{
    private Guid _userId;
    private Guid _familyMemberId;

    [SetUp]
    public async Task LocalSetup()
    {
        _userId = CurrentUser.UserId;
        // Create a family member for testing

        var familyMember = new FamilyMember(_userId, "Test Member", 10);
        Context.FamilyMembers.Add(familyMember);
        await Context.SaveChangesAsync();
        _familyMemberId = familyMember.Id;
    }

    [Test]
    public async Task AddFoodPreference_should_succeed_when_valid()
    {
        var command = new AddFoodPreferenceCommand
        {
            FamilyMemberId = _familyMemberId,
            FoodItem = "Apple",
            Status = PreferenceStatus.Include.ToString()
        };
        var response = await Client.PostAsJsonAsync($"settings/family/{_familyMemberId}/preferences", command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<FamilyMemberResponse>();
        result.Should().NotBeNull();
        result!.FoodPreferences.Should().ContainSingle(fp =>
            fp.FoodItem == "Apple" &&
            fp.Status == PreferenceStatus.Include.ToString());
    }

    [Test]
    public async Task AddFoodPreference_should_return_BadRequest_when_invalid_status()
    {
        var command = new AddFoodPreferenceCommand
        {
            FamilyMemberId = _familyMemberId,
            FoodItem = "Apple",
            Status = "InvalidStatus"
        };
        var response = await Client.PostAsJsonAsync($"settings/family/{_familyMemberId}/preferences", command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task RemoveFoodPreference_should_succeed_when_valid()
    {
        // First add a preference
        var addCommand = new AddFoodPreferenceCommand
        {
            FamilyMemberId = _familyMemberId,
            FoodItem = "Banana",
            Status = PreferenceStatus.Include.ToString()
        };
        var addResponse = await Client.PostAsJsonAsync($"settings/family/{_familyMemberId}/preferences", addCommand);
        addResponse.EnsureSuccessStatusCode();

        // Now remove it
        var response = await Client.DeleteAsync($"settings/family/{_familyMemberId}/preferences/Banana");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<FamilyMemberResponse>();
        result.Should().NotBeNull();
        result!.FoodPreferences.Should().BeEmpty();
    }

    [Test]
    public async Task RemoveFoodPreference_should_return_NotFound_when_not_exists()
    {
        var response = await Client.DeleteAsync($"settings/family/{_familyMemberId}/preferences/NonExistentFood");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task AddFoodPreference_should_return_NotFound_when_family_member_not_exists()
    {
        var command = new AddFoodPreferenceCommand
        {
            FamilyMemberId = System.Guid.NewGuid(),
            FoodItem = "Apple",
            Status = PreferenceStatus.Include.ToString()
        };
        var response = await Client.PostAsJsonAsync($"settings/family/{command.FamilyMemberId}/preferences", command);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}