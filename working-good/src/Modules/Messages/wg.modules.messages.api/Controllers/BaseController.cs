using Microsoft.AspNetCore.Mvc;

namespace wg.modules.messages.api.Controllers;

[ApiController]
[Route($"{MessagesModule.RoutePath}/[controller]")]
internal abstract class BaseController : ControllerBase
{
    
}