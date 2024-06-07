using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace wg.modules.owner.api.Controllers;

[ApiController]
[Route(OwnerModule.RoutePath)]
internal sealed class HomeController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation("Healthcheck for owner module")]
    public ActionResult<string> Get() => "Owner API!";
}