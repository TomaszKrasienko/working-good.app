using Microsoft.AspNetCore.Mvc;
using wg.shared.abstractions.Pagination;
using wg.shared.infrastructure.Serialization;

namespace wg.modules.owner.api.Controllers;

[ApiController]
[Route($"{OwnerModule.RoutePath}/[controller]")]
public class BaseController() : ControllerBase
{
    protected void AddResourceHeader(Guid id)
        => Response.Headers.TryAdd("x-resource-id", id.ToString());
    
    protected void AddPaginationMetaData(MetaDataDto dto)
        => Response.Headers.TryAdd("x-pagination", dto.ToJson());
}