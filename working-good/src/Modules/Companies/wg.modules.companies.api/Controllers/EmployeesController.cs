using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using wg.modules.companies.application.CQRS.Employees.Commands.AddEmployee;
using wg.modules.companies.application.CQRS.Employees.Commands.DeactivateEmployee;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.DTOs;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.infrastructure.Exceptions.DTOs;

namespace wg.modules.companies.api.Controllers;

[Authorize]
internal sealed class EmployeesController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{employeeId:guid}")]
    [ProducesResponseType(typeof(EmployeeDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets employee by \"ID\"")]
    public async Task<ActionResult<EmployeeDto>> GetById(Guid employeeId, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new GetEmployeeByIdQuery(employeeId), cancellationToken);

    [HttpGet("{employeeId:guid}/active")]
    [ProducesResponseType(typeof(EmployeeDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets active employee by \"ID\"")]
    public async Task<ActionResult<EmployeeDto>> GetActiveById(Guid employeeId, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new GetActiveEmployeeByIdQuery(employeeId), cancellationToken);


    [HttpGet("{employeeId:guid}/is-active")]
    [ProducesResponseType(typeof(IsExistsDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [SwaggerOperation("Gets activity of employee by \"ID\"")]
    public async Task<ActionResult<IsExistsDto>> IsEmployeeActiveById(Guid employeeId, CancellationToken cancellationToken)
        => await queryDispatcher.SendAsync(new IsActiveEmployeeExistsQuery(employeeId), cancellationToken);
    
    [HttpPost("companies/{companyId:guid}/add")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Adds employee to company")]
    public async Task<ActionResult> AddEmployee(Guid companyId, AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employeeId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with { CompanyId = companyId, Id = employeeId }, cancellationToken);
        AddResourceHeader(employeeId);
        return CreatedAtAction(nameof(GetById), new { id = companyId }, null);
    }

    [HttpPatch("deactivate/{employeeId:guid}")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(typeof(void),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [SwaggerOperation("Deactivates employee by \"ID\"")]
    public async Task<ActionResult> DeactivateEmployee(Guid employeeId, DeactivateEmployeeCommand command, CancellationToken cancellationToken)
    {
        await commandDispatcher.SendAsync(command with { Id = employeeId }, cancellationToken);
        return Ok();
    }
}