using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.core.Configuration;
using wg.shared.abstractions.Modules;

namespace wg.modules.wiki.api;

internal sealed class WikiModule : IModule
{
    internal const string RoutePath = "wiki-module";
    public string Name { get; } = "Wiki";

    public void Register(IServiceCollection services)
        => services.AddCore();

    public void Use(WebApplication app)
    {
    }
}