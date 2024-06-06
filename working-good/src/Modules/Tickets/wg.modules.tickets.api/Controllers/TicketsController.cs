using System.Collections.Specialized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignEmployee;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignUser;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangePriority;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeProject;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketExpirationDate;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketState;
using wg.modules.tickets.application.CQRS.Tickets.Commands.UpdateTicket;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Context;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.infrastructure.Exceptions.DTOs;
using wg.shared.infrastructure.Pagination.Mappers;

namespace wg.modules.tickets.api.Controllers;

internal sealed class TicketsController(
    IIdentityContext identityContext,
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets all tickets by filters and pagination")]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetAll([FromQuery] GetTicketsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await queryDispatcher.SendAsync(query, cancellationToken);
        var metaData = result.AsMetaData();
        AddPaginationMetaData(metaData);
        return  result.Any() ? Ok(result) : NoContent();
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TicketDto>> GetById(Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetTicketByIdQuery(id), cancellationToken));

    [HttpGet("{ticketId:guid}/is-exists")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IsExistsDto>> IsExists(Guid ticketId, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new IsTicketExistsQuery(ticketId), cancellationToken);
    
    [HttpPost("add")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [SwaggerOperation("Adds ticket")]
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

    [HttpPatch("{ticketId:guid}/update")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Updates ticket")]
    public async Task<ActionResult> UpdateTicket(Guid ticketId, UpdateTicketCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with {Id = ticketId}, cancellationToken);
        return Ok();
    }
    
    [HttpPatch("{ticketId:guid}/employee/{employeeId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Assigns employee to ticket")]
    public async Task<ActionResult> AssignEmployee(Guid ticketId, Guid employeeId, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(new AssignEmployeeCommand(employeeId, ticketId), cancellationToken);
        return Ok();
    }

    [HttpPatch("{id:guid}/user/{userId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Assigns user to ticket")]
    public async Task<ActionResult> AssignUser(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(new AssignUserCommand(userId, id), cancellationToken);
        return Ok();
    }

    [HttpPatch("{id}/change-priority")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Changes ticket priority")]
    public async Task<ActionResult> ChangePriority(Guid id, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(new ChangePriorityCommand(id), cancellationToken);
        return Ok();
    }
    
    [HttpPatch("{id:guid}/change-status")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Changes ticket status")]
    public async Task<ActionResult> ChangeTicketState(Guid id, ChangeTicketStatusCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = id }, cancellationToken);
        return Ok();
    }

    [HttpPatch("{id:guid}/change-expiration-date")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Changes expiration date")]
    public async Task<ActionResult> ChangeExpirationDate(Guid id, ChangeTicketExpirationDateCommand command,
        CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = id }, cancellationToken);
        return Ok();
    }

    [HttpPatch("{id:guid}/project/{projectId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Assigns project to ticket")]
    public async Task<ActionResult> AssignProject(Guid id, Guid projectId, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(new ChangeProjectCommand(id, projectId), cancellationToken);
        return Ok();
    }
    
}