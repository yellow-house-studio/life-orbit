using MediatR;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Common.Exceptions;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;

namespace YellowHouseStudio.LifeOrbit.Application.Family.RemoveSafeFood;

public class RemoveSafeFoodCommandHandler : IRequestHandler<RemoveSafeFoodCommand, SafeFoodResult>
{
    private readonly IFamilyMemberRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<RemoveSafeFoodCommandHandler> _logger;

    public RemoveSafeFoodCommandHandler(
        IFamilyMemberRepository repository,
        ICurrentUser currentUser,
        ILogger<RemoveSafeFoodCommandHandler> logger)
    {
        _repository = repository;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<SafeFoodResult> Handle(RemoveSafeFoodCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Removing safe food {FoodItem} for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);

        var familyMember = await _repository.GetByIdAsync(request.FamilyMemberId, _currentUser.UserId, cancellationToken);
        if (familyMember == null)
        {
            _logger.LogWarning("Family member {FamilyMemberId} not found for user {UserId}", request.FamilyMemberId, _currentUser.UserId);
            throw new NotFoundException($"Family member {request.FamilyMemberId} not found");
        }

        var safeFood = familyMember.SafeFoods.FirstOrDefault(sf => sf.FoodItem == request.FoodItem);
        if (safeFood == null)
        {
            _logger.LogWarning("Safe food {FoodItem} not found for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);
            throw new NotFoundException($"Safe food {request.FoodItem} not found for family member {request.FamilyMemberId}");
        }

        _repository.TrackRemoveSafeFood(familyMember, safeFood);
        // Save is handled by transaction behavior

        _logger.LogInformation("Successfully removed safe food {FoodItem} for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);
        return new SafeFoodResult
        {
            FamilyMemberId = familyMember.Id,
            SafeFoods = familyMember.SafeFoods.Select(sf => sf.FoodItem).ToList()
        };
    }
}