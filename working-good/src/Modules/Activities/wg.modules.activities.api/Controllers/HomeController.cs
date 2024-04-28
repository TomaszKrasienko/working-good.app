using Microsoft.AspNetCore.Mvc;

namespace wg.modules.activities.api.Controllers;

[Route(ActivitiesModule.RoutePath)]
internal sealed class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Activities API!";
}