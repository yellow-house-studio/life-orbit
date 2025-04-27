using NUnit.Framework;
using FluentAssertions;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using YellowHouseStudio.LifeOrbit.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace YellowHouseStudio.LifeOrbit.Tests.Integration.Application.Family;

[TestFixture]
public class FamilyMemberRepositoryTests
{
    private ApplicationDbContext _context = null!;
    private FamilyMemberRepository _repo = null!;
    private FamilyMember _member = null!;
    private Guid _userId;
    private Guid _memberId;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _repo = new FamilyMemberRepository(_context);
        _userId = Guid.NewGuid();
        _member = new FamilyMember(_userId, "Test", 10);
        _memberId = _member.Id;
        _context.FamilyMembers.Add(_member);
        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task HasSafeFoodAsync_should_return_false_when_not_present()
    {
        var result = await _repo.HasSafeFoodAsync(_memberId, "Apple", CancellationToken.None);
        result.Should().BeFalse();
    }

    [Test]
    public async Task TrackNewSafeFood_should_add_safe_food_and_HasSafeFoodAsync_should_return_true()
    {
        var safeFood = new SafeFood("Apple");
        _repo.TrackNewSafeFood(_member, safeFood);
        await _context.SaveChangesAsync();
        var result = await _repo.HasSafeFoodAsync(_memberId, "Apple", CancellationToken.None);
        result.Should().BeTrue();
    }

    [Test]
    public async Task TrackNewSafeFood_should_allow_removal_and_HasSafeFoodAsync_should_return_false()
    {
        var safeFood = new SafeFood("Apple");
        _repo.TrackNewSafeFood(_member, safeFood);
        await _context.SaveChangesAsync();
        _member.RemoveSafeFood("Apple");
        await _context.SaveChangesAsync();
        var result = await _repo.HasSafeFoodAsync(_memberId, "Apple", CancellationToken.None);
        result.Should().BeFalse();
    }
}