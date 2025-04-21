using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.Common;

public record AllergyDto
{
    public string Allergen { get; init; } = string.Empty;
    public string Severity { get; init; } = string.Empty;

    public static AllergyDto FromDomain(Allergy allergy) => new()
    {
        Allergen = allergy.Allergen,
        Severity = allergy.Severity.ToString()
    };
}

public record SafeFoodDto
{
    public string FoodItem { get; init; } = string.Empty;

    public static SafeFoodDto FromDomain(SafeFood safeFood) => new()
    {
        FoodItem = safeFood.FoodItem
    };
}

public record FoodPreferenceDto
{
    public string FoodItem { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;

    public static FoodPreferenceDto FromDomain(FoodPreference preference) => new()
    {
        FoodItem = preference.FoodItem,
        Status = preference.Status.ToString()
    };
}

public record FamilyMemberDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public List<AllergyDto> Allergies { get; init; } = new();
    public List<SafeFoodDto> SafeFoods { get; init; } = new();
    public List<FoodPreferenceDto> FoodPreferences { get; init; } = new();

  
}

public record FamilyMemberResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public List<AllergyDto> Allergies { get; init; } = new();
    public List<SafeFoodDto> SafeFoods { get; init; } = new();
    public List<FoodPreferenceDto> FoodPreferences { get; init; } = new();

      public static FamilyMemberResponse FromDomain(FamilyMember familyMember) => new()
    {
        Id = familyMember.Id,
        Name = familyMember.Name,
        Age = familyMember.Age,
        Allergies = familyMember.Allergies.Select(AllergyDto.FromDomain).ToList(),
        SafeFoods = familyMember.SafeFoods.Select(SafeFoodDto.FromDomain).ToList(),
        FoodPreferences = familyMember.FoodPreferences.Select(FoodPreferenceDto.FromDomain).ToList()
    };
} 