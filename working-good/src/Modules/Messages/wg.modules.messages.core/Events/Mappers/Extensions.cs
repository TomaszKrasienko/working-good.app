using wg.modules.messages.core.Models;

namespace wg.modules.messages.core.Events.Mappers;

internal static class Extensions
{
    internal static MessageReceived AsEvent(this ClientMessage clientMessage)
        => new MessageReceived(clientMessage.Sender, clientMessage.Subject, clientMessage.Content,
            clientMessage.CreatedAt, clientMessage.AssignedEmployee, clientMessage.Number);
}