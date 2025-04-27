using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.Common;

/// <summary>
/// Repository for managing family member entities and their related data.
/// </summary>
public interface IFamilyMemberRepository
{
    /// <summary>
    /// Retrieves a family member by their ID and user ID.
    /// </summary>
    /// <param name="id">The ID of the family member to retrieve.</param>
    /// <param name="userId">The ID of the user who owns the family member record.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The family member if found; otherwise, null.</returns>
    Task<FamilyMember?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a family member by their ID and user ID, including their allergies.
    /// </summary>
    /// <param name="id">The ID of the family member to retrieve.</param>
    /// <param name="userId">The ID of the user who owns the family member record.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The family member with allergies if found; otherwise, null.</returns>
    Task<FamilyMember?> GetByIdWithAllergiesAsync(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if a family member exists for the specified user.
    /// </summary>
    /// <param name="id">The ID of the family member to check.</param>
    /// <param name="userId">The ID of the user who should own the family member record.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>True if the family member exists for the user; otherwise, false.</returns>
    Task<bool> ExistsAsync(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if a family member has a specific allergy.
    /// </summary>
    /// <param name="id">The ID of the family member to check.</param>
    /// <param name="allergen">The allergen to check for.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>True if the family member has the allergy; otherwise, false.</returns>
    Task<bool> HasAllergyAsync(Guid id, string allergen, CancellationToken cancellationToken);

    /// <summary>
    /// Tracks a new allergy for a family member in the current unit of work.
    /// </summary>
    /// <param name="familyMember">The family member to add the allergy to.</param>
    /// <param name="allergy">The allergy to track.</param>
    /// <remarks>
    /// This method handles the entity tracking for the new allergy entity.
    /// The changes will be persisted when the unit of work is committed.
    /// </remarks>
    void TrackNewAllergy(FamilyMember familyMember, Allergy allergy);
}