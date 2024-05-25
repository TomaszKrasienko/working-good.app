using Microsoft.Extensions.DependencyInjection;
using wg.modules.notifications.core.Services.Abstractions;

namespace wg.modules.notifications.core.Services.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IEmailPublisher, FakeEmailPublisher>();
}