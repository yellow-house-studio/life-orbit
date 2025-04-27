using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Users;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Family.AddAllergy;

public class AddAllergyCommandHandler : IRequestHandler<AddAllergyCommand, FamilyMemberResponse>
{
    private readonly IFamilyMemberRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<AddAllergyCommandHandler> _logger;

    public AddAllergyCommandHandler(
        IFamilyMemberRepository repository,
        ICurrentUser currentUser,
        ILogger<AddAllergyCommandHandler> logger)
    {
        _repository = repository;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<FamilyMemberResponse> Handle(AddAllergyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Adding allergy {Allergen} for family member {FamilyMemberId}",
            request.Allergen, request.FamilyMemberId);

        var familyMember = await _repository.GetByIdWithAllergiesAsync(
            request.FamilyMemberId,
            _currentUser.UserId,
            cancellationToken);

        if (familyMember == null)
        {
            _logger.LogWarning("Family member {FamilyMemberId} not found for user {UserId}",
                request.FamilyMemberId, _currentUser.UserId);
            throw new ValidationException("Validation failed", new[]
            {
                new FluentValidation.Results.ValidationFailure("FamilyMemberId", "Family member not found")
            });
        }

        if (!Enum.TryParse<AllergySeverity>(request.Severity, out var severity))
        {
            throw new ValidationException("Validation failed", new[]
            {
                new FluentValidation.Results.ValidationFailure("Severity", "Please select a valid allergy severity level")
            });
        }

        _logger.LogInformation("Adding new allergy {Allergen} for family member {FamilyMemberId}",
            request.Allergen, request.FamilyMemberId);
        var newAllergy = new Allergy(request.Allergen, severity);
        _repository.TrackNewAllergy(familyMember, newAllergy);

        _logger.LogInformation("Successfully added allergy {Allergen} for family member {FamilyMemberId}",
            request.Allergen, request.FamilyMemberId);
        return FamilyMemberResponse.FromDomain(familyMember);
    }
}