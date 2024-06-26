using Microsoft.AspNetCore.Mvc;

namespace wg.modules.messages.api.Controllers;

[Route(MessagesModule.RoutePath)]
internal sealed class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Messages API!";
}