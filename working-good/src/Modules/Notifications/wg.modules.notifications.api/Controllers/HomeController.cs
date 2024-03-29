using Microsoft.AspNetCore.Mvc;

namespace wg.modules.notifications.api.Controllers;

[Route(NotificationsModule.RoutePath)]
internal sealed class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Notifications API!";
}