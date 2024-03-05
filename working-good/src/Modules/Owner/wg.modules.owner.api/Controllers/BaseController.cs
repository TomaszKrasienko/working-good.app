using Microsoft.AspNetCore.Mvc;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.api.Controllers;

[ApiController]
[Route($"{OwnerModule.RoutePath}/[controller]")]
public class BaseController() : ControllerBase
{

    protected void AddResourceHeader(Guid id)
        => Response.Headers.TryAdd("resource-id", id.ToString());
}