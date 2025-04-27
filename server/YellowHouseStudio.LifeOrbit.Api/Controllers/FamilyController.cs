using MediatR;
using Microsoft.AspNetCore.Mvc;
using YellowHouseStudio.LifeOrbit.Application.Family.AddFamilyMember;
using YellowHouseStudio.LifeOrbit.Application.Family.GetFamilyMembers;
using YellowHouseStudio.LifeOrbit.Application.Family.AddCompleteFamilyMember;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;
using YellowHouseStudio.LifeOrbit.Application.Family.UpdateAllergy;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveAllergy;
using YellowHouseStudio.LifeOrbit.Application.Family.AddSafeFood;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveSafeFood;
using YellowHouseStudio.LifeOrbit.Application.Family.UpdateFoodPreference;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveFoodPreference;

namespace YellowHouseStudio.LifeOrbit.Api.Controllers;

[ApiController]
[Route("settings/[controller]")]
public class FamilyController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<FamilyMemberResponse>>> GetFamilyMembers()
    {
        var query = new GetFamilyMembersQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> AddFamilyMember(AddFamilyMemberCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("complete")]
    public async Task<ActionResult<FamilyMemberResponse>> AddCompleteFamilyMember(AddCompleteFamilyMemberCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"settings/family/{result.Id}", result);
    }

    [HttpPut("{id}/allergies")]
    public async Task<ActionResult<FamilyMemberResponse>> UpdateAllergy(Guid id, UpdateAllergyCommand command)
    {
        if (id != command.FamilyMemberId)
        {
            return BadRequest();
        }
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}/allergies/{allergen}")]
    public async Task<ActionResult<FamilyMemberResponse>> RemoveAllergy(Guid id, string allergen)
    {
        var command = new RemoveAllergyCommand { FamilyMemberId = id, Allergen = allergen };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/safe-foods")]
    public async Task<ActionResult<FamilyMemberResponse>> AddSafeFood(Guid id, AddSafeFoodCommand command)
    {
        if (id != command.FamilyMemberId)
        {
            return BadRequest();
        }
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}/safe-foods/{foodItem}")]
    public async Task<ActionResult<FamilyMemberResponse>> RemoveSafeFood(Guid id, string foodItem)
    {
        var command = new RemoveSafeFoodCommand { FamilyMemberId = id, FoodItem = foodItem };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/preferences")]
    public async Task<ActionResult<FamilyMemberResponse>> UpdateFoodPreference(Guid id, UpdateFoodPreferenceCommand command)
    {
        if (id != command.FamilyMemberId)
        {
            return BadRequest();
        }
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}/preferences/{foodItem}")]
    public async Task<ActionResult<FamilyMemberResponse>> RemoveFoodPreference(Guid id, string foodItem)
    {
        var command = new RemoveFoodPreferenceCommand { FamilyMemberId = id, FoodItem = foodItem };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
} 