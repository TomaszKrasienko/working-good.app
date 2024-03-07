using Microsoft.AspNetCore.Mvc;

namespace wg.modules.companies.api.Controllers;

[ApiController]
[Route($"{CompaniesModule.RoutePath}/[controller]")]
internal abstract class BaseController : ControllerBase
{
    
}