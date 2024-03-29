using Microsoft.AspNetCore.Mvc;

namespace wg.modules.notifications.api.Controllers;

[Microsoft.AspNetCore.Components.Route(NotificationsModule.RoutePath)]
internal sealed class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Messages API!";
}