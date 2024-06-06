using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.wiki.application.CQRS.Sections.Commands;
using wg.modules.wiki.application.CQRS.Sections.Commands.ChangeParent;
using wg.modules.wiki.application.CQRS.Sections.Queries;
using wg.modules.wiki.application.DTOs;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.infrastructure.Exceptions.DTOs;

namespace wg.modules.wiki.api.Controllers;

internal sealed class SectionsController(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{sectionId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<SectionDto>> GetById(Guid sectionId, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new GetSectionByIdQuery(sectionId), cancellationToken);
    
    [HttpPost("add")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Add(AddSectionCommand command, CancellationToken cancellationToken)
    {
        var sectionId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with { Id = sectionId }, cancellationToken);
        AddResourceHeader(sectionId);
        return CreatedAtAction(nameof(GetById), new {sectionId = sectionId}, null);
    }

    [HttpPatch("{sectionId:guid}/change-parent")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Changes parent for section")]
    public async Task<ActionResult> ChangeParent(Guid sectionId, ChangeParentCommand command,
        CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with {SectionId = sectionId}, cancellationToken);
        return Ok();
    }
}