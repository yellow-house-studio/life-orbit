using MediatR;
using Microsoft.AspNetCore.Mvc;
using YellowHouseStudio.LifeOrbit.Application.Family.AddFamilyMember;
using YellowHouseStudio.LifeOrbit.Application.Family.GetFamilyMembers;

namespace YellowHouseStudio.LifeOrbit.Api.Controllers;

[ApiController]
[Route("settings/[controller]")]
public class FamilyController : ControllerBase
{
    private readonly IMediator _mediator;

    public FamilyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<FamilyMemberResponse>>> GetFamilyMembers([FromQuery] Guid userId)
    {
        var query = new GetFamilyMembersQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> AddFamilyMember(AddFamilyMemberCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
} 