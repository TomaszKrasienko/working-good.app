using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.notifications.core.Configuration;
using wg.shared.abstractions.Modules;

namespace wg.modules.notifications.api;

internal sealed class NotificationsModule : IModule
{
    public string Name { get; } = "notifications-module";
    public void Register(IServiceCollection services)
    {
        services.AddCore();
    }

    public void Use(WebApplication app)
    {
    }
}