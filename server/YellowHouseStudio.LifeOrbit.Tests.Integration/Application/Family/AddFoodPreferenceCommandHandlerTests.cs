using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.AddFoodPreference;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Common.Exceptions;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using YellowHouseStudio.LifeOrbit.Infrastructure.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using YellowHouseStudio.LifeOrbit.Tests.Common.Users;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration.Application.Family;

[TestFixture]
public class AddFoodPreferenceCommandHandlerTests : TestBase
{
    private AddFoodPreferenceCommandHandler _handler = null!;
    private FamilyMemberRepository _repository = null!;
    private ICurrentUser _currentUser = null!;
    private Guid _userId;

    [SetUp]
    public void Setup()
    {
        _userId = Guid.NewGuid();
        _currentUser = new TestCurrentUser(_userId);
        _repository = new FamilyMemberRepository(Context);
        _handler = new AddFoodPreferenceCommandHandler(_repository, _currentUser, NullLogger<AddFoodPreferenceCommandHandler>.Instance);
    }

    [Test]
    public async Task Handle_should_add_food_preference_when_valid()
    {
        // Arrange
        var familyMember = new FamilyMember(_userId, "Test", 10);
        Context.FamilyMembers.Add(familyMember);
        await Context.SaveChangesAsync();

        var command = new AddFoodPreferenceCommand
        {
            FamilyMemberId = familyMember.Id,
            FoodItem = "Apple",
            Status = PreferenceStatus.Include.ToString()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.FoodPreferences.Should().ContainSingle(fp => fp.FoodItem == "Apple" && fp.Status == PreferenceStatus.Include.ToString());
    }

    [Test]
    public void Handle_should_throw_NotFoundException_when_family_member_not_found()
    {
        // Arrange
        var command = new AddFoodPreferenceCommand
        {
            FamilyMemberId = Guid.NewGuid(),
            FoodItem = "Apple",
            Status = PreferenceStatus.Include.ToString()
        };

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task Handle_should_add_duplicate_food_preference_and_update_status()
    {
        // Arrange
        var familyMember = new FamilyMember(_userId, "Test", 10);
        var existing = new FoodPreference("Apple", PreferenceStatus.AvailableForOthers);
        familyMember.FoodPreferences.Add(existing);
        Context.FamilyMembers.Add(familyMember);
        Context.Entry(existing).State = EntityState.Added;
        await Context.SaveChangesAsync();

        var command = new AddFoodPreferenceCommand
        {
            FamilyMemberId = familyMember.Id,
            FoodItem = "Apple",
            Status = PreferenceStatus.NotAllowed.ToString()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.FoodPreferences.Should().ContainSingle(fp => fp.FoodItem == "Apple" && fp.Status == PreferenceStatus.NotAllowed.ToString());
    }
}