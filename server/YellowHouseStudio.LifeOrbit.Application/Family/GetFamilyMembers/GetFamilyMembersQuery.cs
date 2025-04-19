using MediatR;

namespace YellowHouseStudio.LifeOrbit.Application.Family.GetFamilyMembers;

public record GetFamilyMembersQuery(Guid UserId) : IRequest<List<FamilyMemberResponse>>;

public class FamilyMemberResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public List<AllergyResponse> Allergies { get; set; } = new();
    public List<SafeFoodResponse> SafeFoods { get; set; } = new();
    public List<FoodPreferenceResponse> FoodPreferences { get; set; } = new();
}

public class AllergyResponse
{
    public string Allergen { get; set; } = null!;
    public string Severity { get; set; } = null!;
}

public class SafeFoodResponse
{
    public string FoodItem { get; set; } = null!;
}

public class FoodPreferenceResponse
{
    public string FoodItem { get; set; } = null!;
    public string Status { get; set; } = null!;
} 