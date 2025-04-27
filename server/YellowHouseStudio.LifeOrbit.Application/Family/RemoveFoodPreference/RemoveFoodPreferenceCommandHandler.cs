using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Common.Exceptions;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.RemoveFoodPreference;

public class RemoveFoodPreferenceCommandHandler : IRequestHandler<RemoveFoodPreferenceCommand, FamilyMemberResponse>
{
    private readonly IFamilyMemberRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<RemoveFoodPreferenceCommandHandler> _logger;

    public RemoveFoodPreferenceCommandHandler(
        IFamilyMemberRepository repository,
        ICurrentUser currentUser,
        ILogger<RemoveFoodPreferenceCommandHandler> logger)
    {
        _repository = repository;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<FamilyMemberResponse> Handle(RemoveFoodPreferenceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Removing food preference {FoodItem} for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);

        var familyMember = await _repository.GetByIdAsync(request.FamilyMemberId, _currentUser.UserId, cancellationToken);

        if (familyMember == null)
        {
            _logger.LogWarning("Family member {FamilyMemberId} not found for user {UserId}", request.FamilyMemberId, _currentUser.UserId);
            throw new NotFoundException($"Family member {request.FamilyMemberId} not found");
        }

        var foodPreference = familyMember.FoodPreferences.FirstOrDefault(fp => fp.FoodItem.Equals(request.FoodItem, StringComparison.OrdinalIgnoreCase));
        if (foodPreference == null)
        {
            _logger.LogWarning("Food preference {FoodItem} not found for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);
            throw new NotFoundException($"Food preference {request.FoodItem} not found for family member {request.FamilyMemberId}");
        }

        _repository.TrackRemoveFoodPreference(familyMember, foodPreference);

        _logger.LogInformation("Successfully removed food preference {FoodItem} for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);
        return FamilyMemberResponse.FromDomain(familyMember);
    }
}