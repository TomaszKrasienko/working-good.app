using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace wg.modules.companies.api.Controllers;

[ApiController]
[Route(CompaniesModule.RoutePath)]
internal sealed class HomeController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
    [SwaggerOperation("Healthcheck for companies module")]
    public ActionResult<string> Get() => "Companies API!";
}