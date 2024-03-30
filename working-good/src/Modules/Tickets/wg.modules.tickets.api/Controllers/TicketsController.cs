using System.Collections.Specialized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.shared.abstractions.Context;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.api.Controllers;

internal sealed class TicketsController(
    IIdentityContext identityContext,
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TicketDto>> GetById(Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetTicketByIdQuery(id), cancellationToken));
    
    [HttpPost("add")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddTicket(AddTicketCommand command, CancellationToken cancellationToken)
    {
        var ticketId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with
        {
            Id = ticketId,
            CreatedBy = identityContext.UserId
        }, cancellationToken);
        AddResourceHeader(ticketId);
        return CreatedAtAction(nameof(GetById), new { id = ticketId }, null);
    }
    
}