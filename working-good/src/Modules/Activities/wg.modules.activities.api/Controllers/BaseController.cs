using Microsoft.AspNetCore.Mvc;
using wg.shared.abstractions.Pagination;
using wg.shared.infrastructure.Serialization;

namespace wg.modules.activities.api.Controllers;

[ApiController]
[Route($"{ActivitiesModule.RoutePath}/[controller]")]
internal abstract class BaseController : ControllerBase
{
    protected void AddResourceHeader(Guid id)
        => Response.Headers.TryAdd("x-resource-id", id.ToString());
}