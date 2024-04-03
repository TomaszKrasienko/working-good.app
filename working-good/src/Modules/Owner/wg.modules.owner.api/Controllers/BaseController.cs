using Microsoft.AspNetCore.Mvc;

namespace wg.modules.owner.api.Controllers;

[ApiController]
[Route($"{OwnerModule.RoutePath}/[controller]")]
public class BaseController() : ControllerBase
{

    protected void AddResourceHeader(Guid id)
        => Response.Headers.TryAdd("x-resource-id", id.ToString());
}