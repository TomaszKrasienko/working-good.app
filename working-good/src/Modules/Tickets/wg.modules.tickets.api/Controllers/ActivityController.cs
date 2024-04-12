using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.tickets.application.CQRS.Activities.Commands.AddActivity;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.api.Controllers;

internal sealed class ActivitiesController(
    ICommandDispatcher commandDispatcher) : BaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [HttpPost("ticket/{ticketId:guid}/add")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> AddActivity(AddActivityCommand command, Guid ticketId,
        CancellationToken cancellationToken)
    {
        var activityId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with
        {
            Id = activityId,
            TicketId = ticketId
        }, cancellationToken);
        AddResourceHeader(activityId);
        return CreatedAtAction(nameof(GetById), new {id = activityId }, null);
    }
}