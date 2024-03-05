using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.modules.owner.api.Controllers;

internal sealed class UsersController(
    ICommandDispatcher commandDispatcher, 
    IQueryDispatcher queryDispatcher) : BaseController()
{

}