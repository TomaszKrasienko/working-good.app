using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace wg.modules.wiki.api.Controllers;

[ApiController]
[Route(WikiModule.RoutePath)]
internal sealed class HomeController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
    [SwaggerOperation("Healthcheck for wiki module")]
    public ActionResult<string> Get() => "Wiki API!";
}