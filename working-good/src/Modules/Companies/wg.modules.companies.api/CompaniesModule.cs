using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.companies.infrastructure.Configuration;
using wg.shared.abstractions.Modules;

namespace wg.modules.companies.api;

internal sealed class CompaniesModule : IModule
{
    internal const string RoutePath = "companies-module";
    public string Name { get; } = "Companies";
    public void Register(IServiceCollection services)
    {
        services.AddInfrastructure();
    }

    public void Use(WebApplication app)
    {
    }
}