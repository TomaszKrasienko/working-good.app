using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace wg.modules.tickets.api.Controllers;

[ApiController]
[Route(TicketsModule.RoutePath)]
internal sealed class HomeController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation("Healthcheck for tickets module")]
    public ActionResult<string> Get() => "Tickets API!";
}