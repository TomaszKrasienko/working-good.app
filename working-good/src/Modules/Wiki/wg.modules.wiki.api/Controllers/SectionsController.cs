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

[Authorize]
internal sealed class SectionsController(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{sectionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets section by \"ID\"")]
    public async Task<ActionResult<SectionDto>> GetById(Guid sectionId, CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new GetSectionByIdQuery(sectionId), cancellationToken));
    
    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Adds section")]
    public async Task<ActionResult> Add(AddSectionCommand command, CancellationToken cancellationToken)
    {
        var sectionId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with { Id = sectionId }, cancellationToken);
        AddResourceHeader(sectionId);
        return CreatedAtAction(nameof(GetById), new {sectionId = sectionId}, null);
    }

    [HttpPatch("{sectionId:guid}/change-parent")]
    [ProducesResponseType(typeof(void),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Changes parent for section")]
    public async Task<ActionResult> ChangeParent(Guid sectionId, ChangeParentCommand command,
        CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with {SectionId = sectionId}, cancellationToken);
        return Ok();
    }
}