using wg.shared.abstractions.Messaging;
using wg.shared.infrastructure.Messaging.Channels;

namespace wg.shared.infrastructure.Messaging;

internal sealed class AsyncMessageDispatcher(
    IMessageChannel messageChannel) : IAsyncMessageDispatcher
{
    public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage
        => await messageChannel.Writer.WriteAsync(message);
}