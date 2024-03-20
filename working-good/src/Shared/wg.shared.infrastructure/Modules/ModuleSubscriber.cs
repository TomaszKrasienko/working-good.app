using wg.shared.abstractions.Modules;
using wg.shared.infrastructure.Modules.Abstractions;

namespace wg.shared.infrastructure.Modules;

internal sealed class ModuleSubscriber(
    IModuleRegistry moduleRegistry,
    IServiceProvider serviceProvider) : IModuleSubscriber
{
    public IModuleSubscriber Subscribe<TRequest, TResponse>(string path, 
        Func<TRequest, IServiceProvider, Task<TResponse>> action) where TRequest : class where TResponse : class
    {
        moduleRegistry.AddRequestRegistration(path, typeof(TRequest), typeof(TResponse), 
            async request => await action((TRequest)request, serviceProvider));
        return this;
    }
}