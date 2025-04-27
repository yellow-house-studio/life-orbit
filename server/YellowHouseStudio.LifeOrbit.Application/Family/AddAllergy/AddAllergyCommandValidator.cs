using FluentValidation;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddAllergy;

public class AddAllergyCommandValidator : AbstractValidator<AddAllergyCommand>
{
    public AddAllergyCommandValidator(IFamilyMemberRepository repository, ICurrentUser currentUser)
    {
        RuleFor(x => x.FamilyMemberId)
            .NotEmpty()
            .WithMessage("Please select a family member")
            .MustAsync(async (id, cancellation) =>
                await repository.ExistsAsync(id, currentUser.UserId, cancellation))
            .WithMessage("Family member not found")
            .WithErrorCode(ValidationErrorCodes.NotFound);

        RuleFor(x => x.Allergen)
            .NotEmpty()
            .WithMessage("Please enter an allergen")
            .MaximumLength(100)
            .WithMessage("Allergen name cannot be longer than 100 characters")
            .MustAsync(async (command, allergen, cancellation) =>
                !await repository.HasAllergyAsync(command.FamilyMemberId, allergen, cancellation))
            .WithMessage("This allergy is already registered for this family member");

        RuleFor(x => x.Severity)
            .NotEmpty()
            .WithMessage("Please select an allergy severity")
            .Must(severity => Enum.TryParse<AllergySeverity>(severity, out _))
            .WithMessage("Please select a valid allergy severity level");
    }
}