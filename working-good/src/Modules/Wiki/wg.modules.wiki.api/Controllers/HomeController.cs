using Microsoft.AspNetCore.Mvc;

namespace wg.modules.wiki.api.Controllers;

[ApiController]
[Route(WikiModule.RoutePath)]
internal sealed class HomeController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get() => "Wiki API!";
}