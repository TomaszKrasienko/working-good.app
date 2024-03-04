using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.Time;

namespace wg.shared.infrastructure.Time.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddTime(this IServiceCollection services)
        => services.AddSingleton<IClock, Clock>();
}