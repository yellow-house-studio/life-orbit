using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Tests.Common.Builders;

public class FamilyMemberBuilder
{
    private Guid _userId = Guid.NewGuid();
    private string _name = "Test Member";
    private int _age = 30;
    private readonly List<Allergy> _allergies = new();

    public FamilyMemberBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public FamilyMemberBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public FamilyMemberBuilder WithAge(int age)
    {
        _age = age;
        return this;
    }

    public FamilyMemberBuilder WithAllergy(string allergen, AllergySeverity severity)
    {
        var member = Build();
        member.AddAllergy(allergen, severity);
        _allergies.AddRange(member.Allergies);
        return this;
    }

    public FamilyMember Build()
    {
        var member = new FamilyMember(_userId, _name, _age);
        foreach (var allergy in _allergies)
        {
            member.AddAllergy(allergy.Allergen, allergy.Severity);
        }
        return member;
    }
}