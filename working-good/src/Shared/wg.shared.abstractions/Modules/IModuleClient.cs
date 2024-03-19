namespace wg.shared.abstractions.Modules;

public interface IModuleClient
{
    Task PublishAsync(object message);
    Task<TResult> SendAsync<TResult>(string path, object request) where TResult : class;
}