using Microsoft.AspNetCore.Mvc;

namespace wg.modules.activities.api.Controllers;

[Microsoft.AspNetCore.Components.Route(ActivitiesModule.RoutePath)]
internal sealed class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Activities API!";
}