using MediatR;
using Microsoft.EntityFrameworkCore;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
namespace YellowHouseStudio.LifeOrbit.Application.Family.GetFamilyMembers;

public class GetFamilyMembersQueryHandler : IRequestHandler<GetFamilyMembersQuery, List<FamilyMemberResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;
    public GetFamilyMembersQueryHandler(ApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<List<FamilyMemberResponse>> Handle(GetFamilyMembersQuery request, CancellationToken cancellationToken)
    {
        var familyMembers = await _context.FamilyMembers
            .Where(fm => fm.UserId == _currentUser.UserId)
            .Include(fm => fm.Allergies)
            .Include(fm => fm.SafeFoods)
            .Include(fm => fm.FoodPreferences)
            .ToListAsync(cancellationToken);

        return familyMembers.Select(fm => new FamilyMemberResponse
        {
            Id = fm.Id,
            Name = fm.Name,
            Age = fm.Age,
            Allergies = fm.Allergies.Select(a => new AllergyDto
            {
                Allergen = a.Allergen,
                Severity = a.Severity.ToString()
            }).ToList(),
            SafeFoods = fm.SafeFoods.Select(sf => new SafeFoodDto
            {
                FoodItem = sf.FoodItem
            }).ToList(),
            FoodPreferences = fm.FoodPreferences.Select(fp => new FoodPreferenceDto
            {
                FoodItem = fp.FoodItem,
                Status = fp.Status.ToString()
            }).ToList()
        }).ToList();
    }
} 