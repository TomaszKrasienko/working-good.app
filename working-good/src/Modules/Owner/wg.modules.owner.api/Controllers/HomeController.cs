using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace wg.modules.owner.api.Controllers;

[ApiController]
[Route(OwnerModule.RoutePath)]
internal sealed class HomeController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation("Healthcheck for owner module")]
    public ActionResult<string> Get() => "Owner API!";
}