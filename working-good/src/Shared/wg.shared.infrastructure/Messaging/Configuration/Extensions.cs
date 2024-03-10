using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.Messaging;
using wg.shared.infrastructure.Messaging.Channels;

namespace wg.shared.infrastructure.Messaging.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddMessaging(this IServiceCollection services)
        => services.
             AddSingleton<IMessageChannel, MessageChannel>()
            .AddSingleton<IMessageBroker, MessageBroker>()
            .AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>();
}