using wg.shared.abstractions.Modules;

namespace wg.bootstrapper;

internal static class ModuleServiceRegistry
{
    internal static IServiceCollection AddModulesConfiguration(this IServiceCollection services, 
        IList<IModule> modules)
    {
        foreach (var module in modules)
        {
            module.Register(services);
        }
        return services;
    }
    
    internal static WebApplication UseModulesConfiguration(this WebApplication app, 
        IList<IModule> modules)
    {
        foreach (var module in modules)
        {
            module.Use(app);
        }
        return app;
    }
}