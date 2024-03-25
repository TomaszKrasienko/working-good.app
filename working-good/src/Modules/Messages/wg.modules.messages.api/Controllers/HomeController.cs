using Microsoft.AspNetCore.Mvc;

namespace wg.modules.messages.api.Controllers;

internal sealed class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Messages API!";
}