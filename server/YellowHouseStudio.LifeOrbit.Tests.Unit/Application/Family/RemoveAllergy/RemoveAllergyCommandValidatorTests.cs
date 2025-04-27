using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveAllergy;
using YellowHouseStudio.LifeOrbit.Application.Users;

namespace YellowHouseStudio.LifeOrbit.Tests.Unit.Application.Family.RemoveAllergy;

[TestFixture]
public class RemoveAllergyCommandValidatorTests
{
    private Mock<IFamilyMemberRepository> _repositoryMock = null!;
    private Mock<ICurrentUser> _currentUserMock = null!;
    private RemoveAllergyCommandValidator _validator = null!;
    private Guid _userId;
    private Guid _memberId;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IFamilyMemberRepository>();
        _currentUserMock = new Mock<ICurrentUser>();
        _userId = Guid.NewGuid();
        _memberId = Guid.NewGuid();
        _currentUserMock.Setup(x => x.UserId).Returns(_userId);
        _validator = new RemoveAllergyCommandValidator(_repositoryMock.Object, _currentUserMock.Object);
    }

    [Test]
    public async Task Should_fail_when_family_member_id_is_empty()
    {
        var command = new RemoveAllergyCommand { FamilyMemberId = Guid.Empty, Allergen = "Peanuts" };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RemoveAllergyCommand.FamilyMemberId));
    }

    [Test]
    public async Task Should_fail_when_allergen_is_empty()
    {
        var command = new RemoveAllergyCommand { FamilyMemberId = _memberId, Allergen = string.Empty };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RemoveAllergyCommand.Allergen));
    }

    [Test]
    public async Task Should_fail_when_allergen_is_too_long()
    {
        var allergen = new string('a', 101);
        var command = new RemoveAllergyCommand { FamilyMemberId = _memberId, Allergen = allergen };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RemoveAllergyCommand.Allergen));
    }

    [Test]
    public async Task Should_fail_when_family_member_does_not_exist()
    {
        var command = new RemoveAllergyCommand { FamilyMemberId = _memberId, Allergen = "Peanuts" };
        _repositoryMock.Setup(x => x.ExistsAsync(_memberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode == "404");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Family member not found"));
    }

    [Test]
    public async Task Should_fail_when_allergy_does_not_exist()
    {
        var command = new RemoveAllergyCommand { FamilyMemberId = _memberId, Allergen = "Peanuts" };
        _repositoryMock.Setup(x => x.ExistsAsync(_memberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repositoryMock.Setup(x => x.HasAllergyAsync(_memberId, "Peanuts", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorCode == "404");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("not registered"));
    }

    [Test]
    public async Task Should_pass_when_all_fields_are_valid()
    {
        var command = new RemoveAllergyCommand { FamilyMemberId = _memberId, Allergen = "Peanuts" };
        _repositoryMock.Setup(x => x.ExistsAsync(_memberId, _userId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repositoryMock.Setup(x => x.HasAllergyAsync(_memberId, "Peanuts", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }
}