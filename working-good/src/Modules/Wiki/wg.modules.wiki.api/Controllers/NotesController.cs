using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.wiki.application.CQRS.Notes.Commands;
using wg.modules.wiki.application.CQRS.Notes.Queries;
using wg.modules.wiki.application.DTOs;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.wiki.api.Controllers;

[Authorize]
internal sealed class NotesController(
    IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher) : BaseController
{
    [HttpGet("{noteId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NoteDto>> GetById(Guid id, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetNoteByIdQuery(id), cancellationToken));

    [HttpPost("section/{sectionId:guid}/add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Add(Guid sectionId, AddNoteCommand command, CancellationToken cancellationToken)
    {
        var noteId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with { Id = noteId, SectionId = sectionId }, cancellationToken);
        AddResourceHeader(noteId);
        return CreatedAtAction(nameof(GetById), new { noteId = noteId }, null);
    }
}