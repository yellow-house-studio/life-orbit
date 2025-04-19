using MediatR;
using Microsoft.EntityFrameworkCore;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.GetFamilyMembers;

public class GetFamilyMembersQueryHandler : IRequestHandler<GetFamilyMembersQuery, List<FamilyMemberResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetFamilyMembersQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<FamilyMemberResponse>> Handle(GetFamilyMembersQuery request, CancellationToken cancellationToken)
    {
        var familyMembers = await _context.FamilyMembers
            .Where(fm => fm.UserId == request.UserId)
            .Include(fm => fm.Allergies)
            .Include(fm => fm.SafeFoods)
            .Include(fm => fm.FoodPreferences)
            .ToListAsync(cancellationToken);

        return familyMembers.Select(fm => new FamilyMemberResponse
        {
            Id = fm.Id,
            Name = fm.Name,
            Age = fm.Age,
            Allergies = fm.Allergies.Select(a => new AllergyResponse
            {
                Allergen = a.Allergen,
                Severity = a.Severity.ToString()
            }).ToList(),
            SafeFoods = fm.SafeFoods.Select(sf => new SafeFoodResponse
            {
                FoodItem = sf.FoodItem
            }).ToList(),
            FoodPreferences = fm.FoodPreferences.Select(fp => new FoodPreferenceResponse
            {
                FoodItem = fp.FoodItem,
                Status = fp.Status.ToString()
            }).ToList()
        }).ToList();
    }
} 