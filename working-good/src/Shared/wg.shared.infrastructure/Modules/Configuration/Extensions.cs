using Microsoft.Extensions.DependencyInjection;

namespace wg.shared.infrastructure.Modules.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddModules(this IServiceCollection services)
        => services;

    private static IServiceCollection AddModuleLoad(this IServiceCollection services)
        => services;
}