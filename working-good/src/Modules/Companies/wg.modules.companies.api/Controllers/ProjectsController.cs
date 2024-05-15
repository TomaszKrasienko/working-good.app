using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.CQRS.Projects.Commands.AddProject;
using wg.modules.companies.application.CQRS.Projects.Commands.EditProject;
using wg.modules.companies.application.CQRS.Projects.Queries;
using wg.modules.companies.application.DTOs;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.api.Controllers;

internal sealed class ProjectsController(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{projectId:guid}/employee/{employeeId:guid}/active")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IsExistsDto>> IsActiveProjectForEmployeeExists(Guid projectId, Guid employeeId,
        CancellationToken cancellationToken)
        => Ok(await queryDispatcher.SendAsync(new IsProjectInCompanyQuery(employeeId, projectId), cancellationToken));
    
    [HttpPost("companies/{companyId:guid}/add")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddProject(AddProjectCommand command, Guid companyId, CancellationToken cancellationToken)
    {
        var projectId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with
        {
            CompanyId = companyId,
            Id = projectId
        }, cancellationToken);
        AddResourceHeader(projectId);
        return CreatedAtAction(nameof(GetById), new { id = companyId }, null);
    }

    [HttpPut("edit/{id:guid}")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> EditProject(EditProjectCommand command, Guid id, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = id }, cancellationToken);
        return Ok();
    }
}