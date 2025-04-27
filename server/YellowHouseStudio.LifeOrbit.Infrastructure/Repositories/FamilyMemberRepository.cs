using Microsoft.EntityFrameworkCore;
using YellowHouseStudio.LifeOrbit.Application.Data;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Infrastructure.Repositories;

public class FamilyMemberRepository : IFamilyMemberRepository
{
    private readonly ApplicationDbContext _context;

    public FamilyMemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FamilyMember?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return await _context.FamilyMembers
            .FirstOrDefaultAsync(fm => fm.Id == id && fm.UserId == userId, cancellationToken);
    }

    public async Task<FamilyMember?> GetByIdWithAllergiesAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return await _context.FamilyMembers
            .Include(fm => fm.Allergies)
            .FirstOrDefaultAsync(fm => fm.Id == id && fm.UserId == userId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return await _context.FamilyMembers
            .AnyAsync(fm => fm.Id == id && fm.UserId == userId, cancellationToken);
    }

    public async Task<bool> HasAllergyAsync(Guid id, string allergen, CancellationToken cancellationToken)
    {
        return await _context.FamilyMembers
            .Include(fm => fm.Allergies)
            .Where(fm => fm.Id == id)
            .SelectMany(fm => fm.Allergies)
            .AnyAsync(a => a.Allergen.Equals(allergen, StringComparison.OrdinalIgnoreCase), cancellationToken);
    }

    public void TrackNewAllergy(FamilyMember familyMember, Allergy allergy)
    {
        familyMember.Allergies.Add(allergy);
        _context.Entry(allergy).State = EntityState.Added;
    }
}