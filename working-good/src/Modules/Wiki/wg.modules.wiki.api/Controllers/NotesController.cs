using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.wiki.application.CQRS.Notes.Queries;
using wg.modules.wiki.application.DTOs;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.wiki.api.Controllers;

internal sealed class NotesController(
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NoteDto>> GetById(Guid id, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new GetNoteByIdQuery(id), cancellationToken);
}