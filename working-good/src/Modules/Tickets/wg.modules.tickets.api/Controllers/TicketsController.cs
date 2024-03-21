using System.Collections.Specialized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.api.Controllers;

internal sealed class TicketsController(
    ICommandDispatcher commandDispatcher) : BaseController
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("add-ticket")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddTicket(AddTicketCommand command, CancellationToken cancellationToken)
    {
        var ticketId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with { Id = ticketId }, cancellationToken);
        AddResourceHeader(ticketId);
        return CreatedAtAction(nameof(GetById), new { id = ticketId }, null);
    }
    
}