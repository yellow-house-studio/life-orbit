using MediatR;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddCompleteFamilyMember;

public class AddCompleteFamilyMemberCommandHandler : IRequestHandler<AddCompleteFamilyMemberCommand, FamilyMemberResponse>
{
    private readonly ApplicationDbContext _context;

    public AddCompleteFamilyMemberCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FamilyMemberResponse> Handle(AddCompleteFamilyMemberCommand request, CancellationToken cancellationToken)
    {
        var familyMember = new FamilyMember(request.UserId, request.Name, request.Age);

        foreach (var allergy in request.Allergies)
        {
            familyMember.AddAllergy(allergy.Allergen, Enum.Parse<AllergySeverity>(allergy.Severity));
        }

        foreach (var safeFood in request.SafeFoods)
        {
            familyMember.AddSafeFood(safeFood.FoodItem);
        }

        foreach (var preference in request.FoodPreferences)
        {
            familyMember.AddOrUpdatePreference(preference.FoodItem, Enum.Parse<PreferenceStatus>(preference.Status));
        }

        _context.FamilyMembers.Add(familyMember);
        await _context.SaveChangesAsync(cancellationToken);

        return FamilyMemberResponse.FromDomain(familyMember);
    }
} 