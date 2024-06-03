using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.application.Services.Internals;
using wg.modules.wiki.core.Services.Abstractions;

namespace wg.modules.wiki.core.Services.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<ISectionService, SectionService>();
}