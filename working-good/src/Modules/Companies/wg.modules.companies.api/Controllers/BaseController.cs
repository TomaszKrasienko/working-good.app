using Microsoft.AspNetCore.Mvc;

namespace wg.modules.companies.api.Controllers;

[ApiController]
[Route($"{CompaniesModule.RoutePath}/[controller]")]
internal abstract class BaseController : ControllerBase
{
    protected void AddResourceHeader(Guid id)
        => Response.Headers.TryAdd("x-resource-id", id.ToString());
}