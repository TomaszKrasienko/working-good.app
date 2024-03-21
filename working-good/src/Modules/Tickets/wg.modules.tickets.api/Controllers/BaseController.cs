using Microsoft.AspNetCore.Mvc;

namespace wg.modules.tickets.api.Controllers;

[ApiController]
[Route($"{TicketsModule.RoutePath}/[controller]")]
internal abstract class BaseController : ControllerBase
{
    protected void AddResourceHeader(Guid id)
        => Response.Headers.TryAdd("resource-id", id.ToString());
}