using Microsoft.Extensions.DependencyInjection;
using wg.modules.messages.core.Services.Abstractions;

namespace wg.modules.messages.core.Services.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IMessageService, MessageService>();
            // .AddScoped<IMessageSearcher, MessageSearcher>()
            // .AddHostedService<BackgroundSearcher>();
}