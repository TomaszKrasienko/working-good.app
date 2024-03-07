using Microsoft.AspNetCore.Mvc;

namespace wg.modules.companies.api.Controllers;

[Route(CompaniesModule.RoutePath)]
internal sealed class HomeController : BaseController
{
    [HttpGet]
    public ActionResult<string> Get() => "Companies API!";
}