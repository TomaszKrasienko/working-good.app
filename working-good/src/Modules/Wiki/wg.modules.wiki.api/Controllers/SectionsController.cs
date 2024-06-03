using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.wiki.core.DAL;
using wg.modules.wiki.core.DTOs;
using wg.modules.wiki.core.Mappers;
using wg.modules.wiki.core.Services.Abstractions;
using wg.modules.wiki.core.Services.Commands;
using wg.shared.infrastructure.Exceptions.DTOs;

namespace wg.modules.wiki.api.Controllers;

internal sealed class SectionsController(
    ISectionService sectionService,
    WikiDbContext dbContext) : BaseController
{
    [HttpGet("{sectionId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<SectionDto>> GetById(Guid sectionId, CancellationToken cancellationToken)
        => (await dbContext
            .Sections
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(sectionId), cancellationToken)).AsDto();
    
    [HttpPost("add")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Add(AddSectionCommand command, CancellationToken cancellationToken)
    {
        var sectionId = Guid.NewGuid();
        await sectionService.AddAsync(command with { Id = sectionId }, cancellationToken);
        AddResourceHeader(sectionId);
        return CreatedAtAction(nameof(GetById), new {sectionId = sectionId}, null);
    }

    [HttpPatch("{sectionId:guid}/change-parent")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Changes parent for section")]
    public async Task<ActionResult> ChangeParent(Guid sectionId, ChangeParentSectionCommand command,
        CancellationToken cancellationToken)
    {
        await sectionService.ChangeParentAsync(command with {SectionId = sectionId}, cancellationToken);
        return Ok();
    }
}