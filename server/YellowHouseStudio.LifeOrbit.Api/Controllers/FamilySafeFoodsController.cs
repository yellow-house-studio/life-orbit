using MediatR;
using Microsoft.AspNetCore.Mvc;
using YellowHouseStudio.LifeOrbit.Application.Family.AddSafeFood;
using YellowHouseStudio.LifeOrbit.Application.Family.RemoveSafeFood;
using YellowHouseStudio.LifeOrbit.Application.Family.Common;

namespace YellowHouseStudio.LifeOrbit.Api.Controllers;

[ApiController]
[Route("settings/family/{familyMemberId}/safe-foods")]
public class FamilySafeFoodsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Adds a safe food for a family member.
    /// </summary>
    /// <param name="familyMemberId">The ID of the family member</param>
    /// <param name="request">The safe food details</param>
    /// <returns>The updated safe foods result</returns>
    [HttpPut]
    [ProducesResponseType(typeof(SafeFoodResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SafeFoodResult>> AddSafeFood(Guid familyMemberId, [FromBody] AddSafeFoodRequest request)
    {
        var command = new AddSafeFoodCommand { FamilyMemberId = familyMemberId, FoodItem = request.FoodItem };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Removes a safe food from a family member.
    /// </summary>
    /// <param name="familyMemberId">The ID of the family member</param>
    /// <param name="foodItem">The food item to remove</param>
    /// <returns>The updated safe foods result</returns>
    [HttpDelete("{foodItem}")]
    [ProducesResponseType(typeof(SafeFoodResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SafeFoodResult>> RemoveSafeFood(Guid familyMemberId, string foodItem)
    {
        var command = new RemoveSafeFoodCommand { FamilyMemberId = familyMemberId, FoodItem = foodItem };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
} 