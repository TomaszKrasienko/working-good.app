namespace wg.shared.abstractions.Modules;

public interface IModuleClient
{
    Task PublishAsync(object message);
}