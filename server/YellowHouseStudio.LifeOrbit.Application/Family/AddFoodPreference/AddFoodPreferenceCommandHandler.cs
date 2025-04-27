using MediatR;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Common.Exceptions;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddFoodPreference;

public class AddFoodPreferenceCommandHandler : IRequestHandler<AddFoodPreferenceCommand, FamilyMemberResponse>
{
    private readonly IFamilyMemberRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<AddFoodPreferenceCommandHandler> _logger;

    public AddFoodPreferenceCommandHandler(
        IFamilyMemberRepository repository,
        ICurrentUser currentUser,
        ILogger<AddFoodPreferenceCommandHandler> logger)
    {
        _repository = repository;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<FamilyMemberResponse> Handle(AddFoodPreferenceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Adding food preference {FoodItem} ({Status}) for family member {FamilyMemberId}", request.FoodItem, request.Status, request.FamilyMemberId);

        var familyMember = await _repository.GetByIdAsync(request.FamilyMemberId, _currentUser.UserId, cancellationToken);
        if (familyMember == null)
        {
            _logger.LogWarning("Family member {FamilyMemberId} not found for user {UserId}", request.FamilyMemberId, _currentUser.UserId);
            throw new NotFoundException($"Family member {request.FamilyMemberId} not found");
        }

        var status = Enum.Parse<PreferenceStatus>(request.Status);
        var foodPreference = new FoodPreference(request.FoodItem, status);
        _repository.TrackNewFoodPreference(familyMember, foodPreference);

        _logger.LogInformation("Successfully added food preference {FoodItem} ({Status}) for family member {FamilyMemberId}", request.FoodItem, request.Status, request.FamilyMemberId);
        return FamilyMemberResponse.FromDomain(familyMember);
    }
}