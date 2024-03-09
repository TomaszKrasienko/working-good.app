using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace wg.shared.infrastructure.Exceptions.Configuration;

internal static class Extension
{
    internal static IServiceCollection AddExceptionMiddleware(this IServiceCollection services)
        => services.AddSingleton<ExceptionMiddleware>();

    internal static WebApplication UseExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}