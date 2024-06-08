using System.Reflection;
using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using wg.shared.infrastructure.Auth.Configuration;
using wg.shared.infrastructure.Cache.Configuration;
using wg.shared.infrastructure.Configuration.Models;
using wg.shared.infrastructure.Context.Configuration;
using wg.shared.infrastructure.CQRS.Configuration;
using wg.shared.infrastructure.DAL.Configuration;
using wg.shared.infrastructure.Events.Configuration;
using wg.shared.infrastructure.Exceptions.Configuration;
using wg.shared.infrastructure.Logging.Configuration;
using wg.shared.infrastructure.Messaging.Configuration;
using wg.shared.infrastructure.Metrics.Configuration;
using wg.shared.infrastructure.Modules.Configuration;
using wg.shared.infrastructure.Time.Configuration;
using wg.shared.infrastructure.Vault.Configuration;

namespace wg.shared.infrastructure.Configuration;

public static class Extensions
{
    private const string SectionName = "App";
    
    public static IHostBuilder AddInfrastructure(this IHostBuilder hostBuilder, IConfiguration configuration)
        => hostBuilder
            .AddVault(configuration);
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies,
        IConfiguration configuration)
        => services
            .AddModules(assemblies)
            .AddCqrs(assemblies)
            .AddTime()
            .AddAuth(configuration)
            .AddDal(configuration)
            .AddRedis(configuration)
            .AddExceptionMiddleware()
            .AddMessaging()
            .AddEvents(assemblies)
            .AddIdentityContext()
            .AddAppMetrics()
            .AddLogging(assemblies)
            .AddUiDocumentation(configuration)
            .AddBanner(configuration);

    private static IServiceCollection AddUiDocumentation(this IServiceCollection services, IConfiguration configuration)
        => services.AddSwaggerGen(swagger =>
        {
            var options = configuration.GetOptions<AppOptions>(SectionName);
            swagger.EnableAnnotations();
            swagger.CustomSchemaIds(x => x.FullName);
            swagger.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = options.Name,
                Version = "v1"
            });
        });

    private static IServiceCollection AddBanner(this IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = configuration.GetOptions<AppOptions>(SectionName);
        Console.WriteLine(FiggleFonts.Doom.Render(appOptions.Name));
        return services;
    }

    internal static T GetOptions<T>(this IServiceCollection services) where T : class
    {
        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetService<IOptions<T>>().Value;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
        => app
            .UseControllers()
            .UseUiDocumentation()
            .UseAppMetrics()
            .UseExceptionMiddleware()
            .UseAuth();

    public static WebApplicationBuilder UseInfrastructure(this WebApplicationBuilder app)
        => app
            .UseSerilog();
    
    private static WebApplication UseUiDocumentation(this WebApplication app)
    {
        var options = app.Configuration.GetOptions<AppOptions>(SectionName);
        
        app.UseSwagger();
        app.UseSwaggerUI(swagger =>
        {
            swagger.RoutePrefix = "swagger";
            swagger.DocumentTitle = $"{options.Name}.API";
        });
        app.UseReDoc(reDoc =>
        {
            reDoc.RoutePrefix = "redoc";
            reDoc.SpecUrl("/swagger/v1/swagger.json");
            reDoc.DocumentTitle = $"{options.Name}.API";
        });
        return app;
    }

    private static WebApplication UseControllers(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        T t = new T();
        configuration.Bind(sectionName, t);
        return t;
    }
}