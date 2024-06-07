using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace wg.modules.notifications.api.Controllers;

[Route(NotificationsModule.RoutePath)]
internal sealed class HomeController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
    [SwaggerOperation("Healthcheck for notifications module")]
    public ActionResult<string> Get() => "Notifications API!";
}