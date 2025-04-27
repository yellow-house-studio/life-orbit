using NUnit.Framework;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveSafeFood;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration.Application.Family;

[TestFixture]
public class RemoveSafeFoodCommandHandlerTests : TestBase
{
    private Guid _userId;
    private FamilyMember _member = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = ((TestCurrentUser)CurrentUser).UserId;
        _member = new FamilyMember(_userId, "Test", 10);
        Context.FamilyMembers.Add(_member);
        Context.SaveChanges();
    }

    [Test]
    public async Task Handle_removes_safe_food_and_returns_result()
    {
        var apple = new SafeFood("Apple");
        Context.Entry(apple).State = EntityState.Added;
        _member.SafeFoods.Add(apple);
        Context.SaveChanges();
        var command = new RemoveSafeFoodCommand { FamilyMemberId = _member.Id, FoodItem = "Apple" };
        var result = await Mediator.Send(command, CancellationToken.None);
        result.FamilyMemberId.Should().Be(_member.Id);
        result.SafeFoods.Should().NotContain("Apple");
        var saved = await Context.FamilyMembers.FindAsync(_member.Id);
        saved!.SafeFoods.Should().NotContain(sf => sf.FoodItem == "Apple");
    }

    [Test]
    public async Task Handle_throws_NotFoundException_for_missing_member()
    {
        var command = new RemoveSafeFoodCommand { FamilyMemberId = Guid.NewGuid(), FoodItem = "Apple" };
        Func<Task> act = async () => await Mediator.Send(command, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>().Where(e => e.GetType().Name.Contains("NotFound"));
    }

    [Test]
    public async Task Handle_throws_NotFoundException_for_missing_safe_food()
    {
        var command = new RemoveSafeFoodCommand { FamilyMemberId = _member.Id, FoodItem = "Banana" };
        Func<Task> act = async () => await Mediator.Send(command, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>().Where(e => e.GetType().Name.Contains("NotFound"));
    }
} 