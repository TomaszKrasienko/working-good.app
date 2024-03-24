using Microsoft.Extensions.DependencyInjection;
using wg.modules.messages.core.Services.Configuration;

namespace wg.modules.messages.core.Configuration;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
        => services
            .AddServices();
}