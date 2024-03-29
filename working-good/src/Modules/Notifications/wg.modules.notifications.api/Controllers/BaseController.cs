using Microsoft.AspNetCore.Mvc;

namespace wg.modules.notifications.api.Controllers;

[ApiController]
[Route($"{NotificationsModule.RoutePath}/[controller]")]
internal abstract class BaseController : ControllerBase
{
    
}