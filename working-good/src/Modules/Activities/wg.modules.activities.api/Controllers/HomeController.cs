using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace wg.modules.activities.api.Controllers;

[Route(ActivitiesModule.RoutePath)]
internal sealed class HomeController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
    [SwaggerOperation("Healthcheck for companies module")]
    public ActionResult<string> Get() => "Activities API!";
}