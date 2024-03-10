using Microsoft.IdentityModel.Tokens;
using wg.shared.abstractions.Messaging;

namespace wg.shared.infrastructure.Messaging;

internal sealed class MessageBroker(
    IAsyncMessageDispatcher asyncMessageDispatcher) : IMessageBroker
{
    public async Task PublishAsync(params IMessage[] messages)
    {
        if (messages.IsNullOrEmpty())
        {
            return;
        }

        var filledMessages = messages.Where(x => x is not null)
            .ToArray();
        if (!filledMessages.Any())
        {
            return;
        }
        foreach (var message in filledMessages)
        {
            await asyncMessageDispatcher.PublishAsync(message);
        }
    }
}