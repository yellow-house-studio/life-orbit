using NUnit.Framework;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveSafeFood;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Common.Exceptions;

namespace YellowHouseStudio.LifeOrbit.Tests.Unit.Application.Family.RemoveSafeFood;

[TestFixture]
public class RemoveSafeFoodCommandHandlerTests
{
    private Mock<IFamilyMemberRepository> _repoMock = null!;
    private Mock<ICurrentUser> _userMock = null!;
    private Mock<ILogger<RemoveSafeFoodCommandHandler>> _loggerMock = null!;
    private RemoveSafeFoodCommandHandler _handler = null!;
    private Guid _userId;
    private Guid _memberId;

    [SetUp]
    public void SetUp()
    {
        _repoMock = new Mock<IFamilyMemberRepository>();
        _userMock = new Mock<ICurrentUser>();
        _loggerMock = new Mock<ILogger<RemoveSafeFoodCommandHandler>>();
        _handler = new RemoveSafeFoodCommandHandler(_repoMock.Object, _userMock.Object, _loggerMock.Object);
        _userId = Guid.NewGuid();
        _memberId = Guid.NewGuid();
        _userMock.Setup(u => u.UserId).Returns(_userId);
    }

    [Test]
    public async Task Handle_should_throw_NotFoundException_if_member_not_found()
    {
        _repoMock.Setup(r => r.GetByIdAsync(_memberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync((FamilyMember?)null);
        var command = new RemoveSafeFoodCommand { FamilyMemberId = _memberId, FoodItem = "Apple" };
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task Handle_should_throw_NotFoundException_if_safe_food_not_found()
    {
        var member = new FamilyMember(_userId, "Test", 10);
        _repoMock.Setup(r => r.GetByIdAsync(_memberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(member);
        var command = new RemoveSafeFoodCommand { FamilyMemberId = _memberId, FoodItem = "Apple" };
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task Handle_should_remove_safe_food()
    {
        var member = new FamilyMember(_userId, "Test", 10);
        member.AddSafeFood("Apple");
        _repoMock.Setup(r => r.GetByIdAsync(_memberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(member);
        var command = new RemoveSafeFoodCommand { FamilyMemberId = _memberId, FoodItem = "Apple" };
        var result = await _handler.Handle(command, CancellationToken.None);
        _repoMock.Verify(r => r.TrackRemoveSafeFood(member, It.Is<SafeFood>(sf => sf.FoodItem == "Apple")), Times.Once);
    }
} 