using NUnit.Framework;
using FluentAssertions;
using Moq;
using YellowHouseStudio.LifeOrbit.Application.Family.AddSafeFood;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Common.Exceptions;

namespace YellowHouseStudio.LifeOrbit.Tests.Unit.Application.Family.AddSafeFood;

[TestFixture]
public class AddSafeFoodCommandHandlerTests
{
    private Mock<IFamilyMemberRepository> _repoMock = null!;
    private Mock<ICurrentUser> _userMock = null!;
    private Mock<ILogger<AddSafeFoodCommandHandler>> _loggerMock = null!;
    private AddSafeFoodCommandHandler _handler = null!;
    private Guid _userId;
    private Guid _memberId;

    [SetUp]
    public void SetUp()
    {
        _repoMock = new Mock<IFamilyMemberRepository>();
        _userMock = new Mock<ICurrentUser>();
        _loggerMock = new Mock<ILogger<AddSafeFoodCommandHandler>>();
        _handler = new AddSafeFoodCommandHandler(_repoMock.Object, _userMock.Object, _loggerMock.Object);
        _userId = Guid.NewGuid();
        _memberId = Guid.NewGuid();
        _userMock.Setup(u => u.UserId).Returns(_userId);
    }

    [Test]
    public async Task Handle_should_throw_NotFoundException_if_member_not_found()
    {
        _repoMock.Setup(r => r.GetByIdAsync(_memberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync((FamilyMember?)null);
        var command = new AddSafeFoodCommand { FamilyMemberId = _memberId, FoodItem = "Apple" };
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task Handle_should_not_add_duplicate_safe_food()
    {
        var member = new FamilyMember(_userId, "Test", 10);
        member.AddSafeFood("Apple");
        _repoMock.Setup(r => r.GetByIdAsync(_memberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(member);
        var command = new AddSafeFoodCommand { FamilyMemberId = _memberId, FoodItem = "Apple" };
        var result = await _handler.Handle(command, CancellationToken.None);
        result.SafeFoods.Should().ContainSingle(f => f == "Apple");
        _repoMock.Verify(r => r.TrackNewSafeFood(It.IsAny<FamilyMember>(), It.IsAny<SafeFood>()), Times.Never);
    }

    [Test]
    public async Task Handle_should_add_new_safe_food()
    {
        var member = new FamilyMember(_userId, "Test", 10);
        _repoMock.Setup(r => r.GetByIdAsync(_memberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(member);
        _repoMock.Setup(r => r.TrackNewSafeFood(member, It.IsAny<SafeFood>()))
            .Callback<FamilyMember, SafeFood>((fm, sf) => fm.SafeFoods.Add(sf));
        var command = new AddSafeFoodCommand { FamilyMemberId = _memberId, FoodItem = "Banana" };
        var result = await _handler.Handle(command, CancellationToken.None);
        result.SafeFoods.Should().Contain("Banana");
        _repoMock.Verify(r => r.TrackNewSafeFood(member, It.Is<SafeFood>(sf => sf.FoodItem == "Banana")), Times.Once);
    }
}