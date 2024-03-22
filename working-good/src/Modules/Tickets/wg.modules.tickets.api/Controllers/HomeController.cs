using Microsoft.AspNetCore.Mvc;

namespace wg.modules.tickets.api.Controllers;

[ApiController]
[Route(TicketsModule.RoutePath)]
internal sealed class HomeController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get() => "Tickets API!";
}