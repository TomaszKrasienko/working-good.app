using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.wiki.infrastructure.Configuration;
using wg.shared.abstractions.Modules;

namespace wg.modules.wiki.api;

internal sealed class WikiModule : IModule
{
    internal const string RoutePath = "wiki-module";
    public string Name { get; } = "Wiki";

    public void Register(IServiceCollection services)
        => services.AddInfrastructure();

    public void Use(WebApplication app)
    {
    }
}