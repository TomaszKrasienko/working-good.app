using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.tickets.application.CQRS.Messages.Commands.AddMessage;
using wg.modules.tickets.application.CQRS.Messages.Queries;
using wg.modules.tickets.application.DTOs;
using wg.shared.abstractions.Context;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.tickets.api.Controllers;

internal sealed class MessagesController(
    ICommandDispatcher commandDispatcher,   
    IQueryDispatcher queryDispatcher,   
    IIdentityContext identityContext): BaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets message by \"ID\"")]
    public async Task<ActionResult<MessageDto>> GetById(Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetMessageByIdQuery(id), cancellationToken));

    [HttpPost("ticket/{ticketId:guid}/add")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Adds message to ticket")]
    public async Task<ActionResult> AddMessage(AddMessageCommand command, Guid ticketId, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var userId = identityContext.UserId;
        await commandDispatcher.SendAsync(command with { Id = id, TicketId = ticketId, UserId = userId }, cancellationToken);
        AddResourceHeader(id);
        return CreatedAtAction(nameof(GetById), new { id = id }, null);
    }
}