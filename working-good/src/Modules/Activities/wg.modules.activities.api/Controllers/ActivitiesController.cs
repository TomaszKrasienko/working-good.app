using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.activities.application.CQRS.Activities.Commands.AddActivity;
using wg.modules.activities.application.CQRS.Activities.Queries;
using wg.modules.activities.application.DTOs;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.infrastructure.Exceptions.DTOs;

namespace wg.modules.activities.api.Controllers;

[Authorize]
internal sealed class ActivitiesController(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{activityId:guid}")]
    [ProducesResponseType(typeof(ActivityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Gets activity by \"ID\"")]
    public async Task<ActionResult<ActivityDto>> GetById(Guid activityId, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetActivityById(activityId), cancellationToken));

    [HttpGet("ticket/{ticketId:guid}")]
    [ProducesResponseType(typeof(List<ActivityDto>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(Summary = "Gets activities by \"TicketId\"")]
    public async Task<ActionResult<IReadOnlyCollection<ActivityDto>>> GetByTicketId(Guid ticketId, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetActivitiesByTicketIdQueryQuery(ticketId), cancellationToken));
    
    [HttpPost("add")]
    [ProducesResponseType(typeof(void),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Adds new activity")]
    public async Task<ActionResult> AddActivity(AddActivityCommand command, CancellationToken cancellationToken)
    {
        var activityId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with { Id = activityId }, cancellationToken);
        AddResourceHeader(activityId);
        return CreatedAtAction(nameof(GetById), new {id = activityId}, null);
    }
}