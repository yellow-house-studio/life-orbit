using MediatR;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Common.Exceptions;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddSafeFood;

public class AddSafeFoodCommandHandler : IRequestHandler<AddSafeFoodCommand, SafeFoodResult>
{
    private readonly IFamilyMemberRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<AddSafeFoodCommandHandler> _logger;

    public AddSafeFoodCommandHandler(
        IFamilyMemberRepository repository,
        ICurrentUser currentUser,
        ILogger<AddSafeFoodCommandHandler> logger)
    {
        _repository = repository;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<SafeFoodResult> Handle(AddSafeFoodCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Adding safe food {FoodItem} for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);

        var familyMember = await _repository.GetByIdAsync(request.FamilyMemberId, _currentUser.UserId, cancellationToken);
        if (familyMember == null)
        {
            _logger.LogWarning("Family member {FamilyMemberId} not found for user {UserId}", request.FamilyMemberId, _currentUser.UserId);
            throw new NotFoundException($"Family member {request.FamilyMemberId} not found");
        }

        if (familyMember.SafeFoods.Any(sf => sf.FoodItem == request.FoodItem))
        {
            // Already present, no-op
            _logger.LogInformation("Safe food {FoodItem} already present for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);
        }
        else
        {
            var safeFood = new SafeFood(request.FoodItem);
            _repository.TrackNewSafeFood(familyMember, safeFood);
        }

        // Save is handled by transaction behavior
        _logger.LogInformation("Successfully added safe food {FoodItem} for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);
        return new SafeFoodResult
        {
            FamilyMemberId = familyMember.Id,
            SafeFoods = familyMember.SafeFoods.Select(sf => sf.FoodItem).ToList()
        };
    }
}