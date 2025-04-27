using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Common.Exceptions;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddSafeFood;

public class AddSafeFoodCommandHandler : IRequestHandler<AddSafeFoodCommand, FamilyMemberResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<AddSafeFoodCommandHandler> _logger;

    public AddSafeFoodCommandHandler(
        ApplicationDbContext context,
        ICurrentUser currentUser,
        ILogger<AddSafeFoodCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<FamilyMemberResponse> Handle(AddSafeFoodCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Adding safe food {FoodItem} for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);

        var familyMember = await _context.FamilyMembers
            .Include(fm => fm.Allergies)
            .Include(fm => fm.SafeFoods)
            .Include(fm => fm.FoodPreferences)
            .FirstOrDefaultAsync(fm => fm.Id == request.FamilyMemberId && fm.UserId == _currentUser.UserId, cancellationToken);

        if (familyMember == null)
        {
            _logger.LogWarning("Family member {FamilyMemberId} not found for user {UserId}", request.FamilyMemberId, _currentUser.UserId);
            throw new NotFoundException($"Family member {request.FamilyMemberId} not found");
        }

        familyMember.AddSafeFood(request.FoodItem);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully added safe food {FoodItem} for family member {FamilyMemberId}", request.FoodItem, request.FamilyMemberId);
        return FamilyMemberResponse.FromDomain(familyMember);
    }
} 