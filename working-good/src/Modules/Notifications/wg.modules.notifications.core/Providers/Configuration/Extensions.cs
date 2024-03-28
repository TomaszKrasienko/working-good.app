using Microsoft.Extensions.DependencyInjection;
using wg.modules.notifications.core.Providers.Abstractions;

namespace wg.modules.notifications.core.Providers.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddProviders(this IServiceCollection services)
        => services
            .AddSingleton<IEmailNotificationProvider, EmailNotificationProvider>();
}