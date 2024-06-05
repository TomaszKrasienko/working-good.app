using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.application.Strategies.Configuration;

namespace wg.modules.wiki.application.Configuration;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services.AddStrategies();
}