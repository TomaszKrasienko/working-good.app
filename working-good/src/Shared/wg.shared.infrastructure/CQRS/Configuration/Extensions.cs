using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;

namespace wg.shared.infrastructure.CQRS.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddCqrs(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        => services
            .AddCommands(assemblies)
            .AddQueries(assemblies)
            .AddDispatchers();

    private static IServiceCollection AddCommands(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }
    
    private static IServiceCollection AddDispatchers(this IServiceCollection services)
        => services
            .AddSingleton<ICommandDispatcher, CommandDispatcher>()
            .AddSingleton<IQueryDispatcher, QueryDispatcher>();
}