using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wg.modules.messages.core.Services.Abstractions;
using wg.modules.messages.core.Services.Commands;

namespace wg.modules.messages.api.Controllers;

internal sealed class MessagesController(
    IMessageService messageService) : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create(CreateMessage command)
    {
        await messageService.CreateMessage(command);
        return Accepted();
    }
}