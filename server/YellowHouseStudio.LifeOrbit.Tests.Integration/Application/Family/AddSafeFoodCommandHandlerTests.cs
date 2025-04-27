using NUnit.Framework;
using FluentAssertions;
using YellowHouseStudio.LifeOrbit.Application.Family.AddSafeFood;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration.Application.Family;

[TestFixture]
public class AddSafeFoodCommandHandlerTests : TestBase
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
    public async Task Handle_adds_safe_food_and_returns_result()
    {
        var command = new AddSafeFoodCommand { FamilyMemberId = _member.Id, FoodItem = "Apple" };
        var result = await Mediator.Send(command, CancellationToken.None);
        result.FamilyMemberId.Should().Be(_member.Id);
        result.SafeFoods.Should().Contain("Apple");
        var saved = await Context.FamilyMembers.FindAsync(_member.Id);
        saved!.SafeFoods.Should().ContainSingle(sf => sf.FoodItem == "Apple");
    }

    [Test]
    public async Task Handle_duplicate_safe_food_is_noop()
    {
        var apple = new SafeFood("Apple");
        Context.Entry(apple).State = EntityState.Added;
        _member.SafeFoods.Add(apple);
        Context.SaveChanges();
        _member = Context.FamilyMembers.Include(fm => fm.SafeFoods).First(fm => fm.Id == _member.Id);
        var command = new AddSafeFoodCommand { FamilyMemberId = _member.Id, FoodItem = "Apple" };
        var result = await Mediator.Send(command, CancellationToken.None);
        result.SafeFoods.Should().ContainSingle(f => f == "Apple");
    }

    [Test]
    public async Task Handle_throws_NotFoundException_for_missing_member()
    {
        var command = new AddSafeFoodCommand { FamilyMemberId = Guid.NewGuid(), FoodItem = "Apple" };
        Func<Task> act = async () => await Mediator.Send(command, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>().Where(e => e.GetType().Name.Contains("NotFound"));
    }
} 