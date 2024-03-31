using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Events;
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
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(QueryHandlerLogDecorator<,>));
        services.TryDecorate(typeof(IEventHandler<>), typeof(EventHandlerLogDecorator<>));
        return services;
    }

    internal static WebApplicationBuilder UseSerilog(this WebApplicationBuilder app)
    {
        app.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3} {SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Seq("http://localhost:5011"); 
        });
        return app;
    }
}