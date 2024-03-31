using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.Events;

namespace wg.shared.infrastructure.Events.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddEvents(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        => services
            .AddServices()
            .AddEventHandlersScanning(assemblies);

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddSingleton<IEventDispatcher, EventDispatcher>();

    private static IServiceCollection AddEventHandlersScanning(this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        services.Scan(x => x.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>))
                .WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }
}