using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wg.modules.companies.application.CQRS.Companies.AddCompany;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.companies.api.Controllers;

internal sealed class CompaniesController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPost("add")]
    public async Task<ActionResult> AddCompany(AddCompanyCommand command, CancellationToken cancellationToken)
    {
        var companyId = Guid.NewGuid();
        await commandDispatcher.SendAsync(command with { Id = companyId }, cancellationToken);
        AddResourceHeader(companyId);
        return CreatedAtAction(nameof(GetById), new { id = companyId }, null);
    }
}