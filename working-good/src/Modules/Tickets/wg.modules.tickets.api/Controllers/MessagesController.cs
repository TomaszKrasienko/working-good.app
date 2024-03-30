using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.tickets.application.DTOs;

namespace wg.modules.tickets.api.Controllers;

internal sealed class MessagesController : BaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MessageDto>> GetById(Guid id, CancellationToken cancellationToken)
        => Ok();
}