using Microsoft.AspNetCore.Mvc;

namespace wg.modules.activities.api.Controllers;

[ApiController]
[Route($"{ActivitiesModule.RoutePath}/[controller]")]
internal abstract class BaseController : ControllerBase
{
    protected void AddResourceHeader(Guid id)
        => Response.Headers.TryAdd("x-resource-id", id.ToString());
}