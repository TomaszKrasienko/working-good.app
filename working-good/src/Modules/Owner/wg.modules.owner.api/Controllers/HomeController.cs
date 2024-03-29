using Microsoft.AspNetCore.Mvc;

namespace wg.modules.owner.api.Controllers;

[ApiController]
[Route(OwnerModule.RoutePath)]
internal sealed class HomeController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get() => "Owner API!";
}