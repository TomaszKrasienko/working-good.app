namespace wg.shared.abstractions.Modules;

public interface IModuleSubscriber
{
    IModuleSubscriber Subscribe<TRequest, TResponse>(string path,
        Func<TRequest, IServiceProvider, Task<TResponse>> action) where TRequest : class where TResponse : class;
}