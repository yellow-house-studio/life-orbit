using MediatR;
using Microsoft.Extensions.Logging;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddCompleteFamilyMember;

public class AddCompleteFamilyMemberCommandHandler(ApplicationDbContext context, ILogger<AddCompleteFamilyMemberCommandHandler> logger) : IRequestHandler<AddCompleteFamilyMemberCommand, FamilyMemberResponse>
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<AddCompleteFamilyMemberCommandHandler> _logger = logger;

    public async Task<FamilyMemberResponse> Handle(AddCompleteFamilyMemberCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Creating new family member for user {UserId} with name {Name}", request.UserId, request.Name);
        
        var familyMember = new FamilyMember(request.UserId, request.Name, request.Age);

        foreach (var allergy in request.Allergies)
        {
            familyMember.AddAllergy(allergy.Allergen, Enum.Parse<AllergySeverity>(allergy.Severity));
            _logger.LogTrace("Added allergy {Allergen} with severity {Severity}", allergy.Allergen, allergy.Severity);
        }

        foreach (var safeFood in request.SafeFoods)
        {
            familyMember.AddSafeFood(safeFood.FoodItem);
            _logger.LogTrace("Added safe food {FoodItem}", safeFood.FoodItem);
        }

        foreach (var preference in request.FoodPreferences)
        {
            familyMember.AddOrUpdatePreference(preference.FoodItem, Enum.Parse<PreferenceStatus>(preference.Status));
            _logger.LogTrace("Added food preference for {FoodItem} with status {Status}", preference.FoodItem, preference.Status);
        }

        await _context.FamilyMembers.AddAsync(familyMember, cancellationToken);
        
        _logger.LogInformation("Successfully created family member {FamilyMemberId} for user {UserId}", familyMember.Id, familyMember.UserId);
        return FamilyMemberResponse.FromDomain(familyMember);
    }
} 