using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.Context;
using wg.shared.infrastructure.Context.Factories;

namespace wg.shared.infrastructure.Context.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddIdentityContext(this IServiceCollection services)
        => services
            .AddHttpContextAccessor()
            .AddSingleton<IIdentityContextFactory, IdentityContextFactory>()
            .AddScoped<IIdentityContext>(sp =>
            {
                var factory = sp.GetRequiredService<IIdentityContextFactory>();
                return factory.Create();
            });
}