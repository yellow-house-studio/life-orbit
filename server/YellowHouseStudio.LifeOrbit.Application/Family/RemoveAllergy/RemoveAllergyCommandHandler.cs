using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Common.Exceptions;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;

namespace YellowHouseStudio.LifeOrbit.Application.Family.RemoveAllergy;

public class RemoveAllergyCommandHandler : IRequestHandler<RemoveAllergyCommand, FamilyMemberResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<RemoveAllergyCommandHandler> _logger;

    public RemoveAllergyCommandHandler(
        ApplicationDbContext context,
        ICurrentUser currentUser,
        ILogger<RemoveAllergyCommandHandler> logger)
    {
        _context = context;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<FamilyMemberResponse> Handle(RemoveAllergyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Removing allergy {Allergen} for family member {FamilyMemberId}", request.Allergen, request.FamilyMemberId);

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

        familyMember.RemoveAllergy(request.Allergen);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully removed allergy {Allergen} for family member {FamilyMemberId}", request.Allergen, request.FamilyMemberId);
        return FamilyMemberResponse.FromDomain(familyMember);
    }
} 