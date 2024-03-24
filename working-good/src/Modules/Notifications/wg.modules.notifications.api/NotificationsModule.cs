using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.shared.abstractions.Modules;

namespace wg.modules.notifications.api;

internal sealed class NotificationsModule : IModule
{
    public string Name { get; } = "notifications-module";
    public void Register(IServiceCollection services)
    {
    }

    public void Use(WebApplication app)
    {
    }
}