using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.application.Strategies.Origins;

namespace wg.modules.wiki.application.Strategies.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddStrategies(this IServiceCollection services)
        => services
            .AddScoped<IOriginCheckingStrategy, ClientCheckingStrategy>()
            .AddScoped<IOriginCheckingStrategy, TicketCheckingStrategy>();
}