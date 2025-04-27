using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.AddFoodPreference;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Common.Exceptions;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Tests.Unit.Application.Family.AddFoodPreference;

[TestFixture]
public class AddFoodPreferenceCommandHandlerTests
{
    private Mock<IFamilyMemberRepository> _repository = null!;
    private Mock<ICurrentUser> _currentUser = null!;
    private Mock<ILogger<AddFoodPreferenceCommandHandler>> _logger = null!;
    private AddFoodPreferenceCommandHandler _handler = null!;
    private Guid _userId;

    [SetUp]
    public void Setup()
    {
        _userId = Guid.NewGuid();
        _repository = new Mock<IFamilyMemberRepository>();
        _currentUser = new Mock<ICurrentUser>();
        _logger = new Mock<ILogger<AddFoodPreferenceCommandHandler>>();
        _currentUser.Setup(x => x.UserId).Returns(_userId);
        _handler = new AddFoodPreferenceCommandHandler(_repository.Object, _currentUser.Object, _logger.Object);
    }

    [Test]
    public async Task Handle_should_add_food_preference_when_valid()
    {
        // Arrange
        var familyMember = new FamilyMember(_userId, "Test", 10);
        var command = new AddFoodPreferenceCommand
        {
            FamilyMemberId = familyMember.Id,
            FoodItem = "Apple",
            Status = PreferenceStatus.Include.ToString()
        };
        _repository.Setup(x => x.GetByIdAsync(command.FamilyMemberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(familyMember);
        _repository.Setup(x => x.TrackNewFoodPreference(familyMember, It.IsAny<FoodPreference>())).Callback<FamilyMember, FoodPreference>((fm, fp) => fm.FoodPreferences.Add(fp));

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
        _repository.Setup(x => x.GetByIdAsync(command.FamilyMemberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync((FamilyMember?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<NotFoundException>();
    }
}