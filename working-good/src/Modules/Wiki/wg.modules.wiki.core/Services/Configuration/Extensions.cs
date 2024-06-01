using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.core.Services.Abstractions;
using wg.modules.wiki.core.Services.Internals;

namespace wg.modules.wiki.core.Services.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<ISectionService, SectionService>();
}