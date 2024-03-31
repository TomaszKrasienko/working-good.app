using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.infrastructure.Logging.Decorators;

namespace wg.shared.infrastructure.Logging.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddLogging(this IServiceCollection service, IList<Assembly> assemblies)
        => service
            .AddDecorators(assemblies);

    private static IServiceCollection AddDecorators(this IServiceCollection services, IList<Assembly> assemblies)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(CommandHandlerLogDecorator<>));
        return services;
    }
}