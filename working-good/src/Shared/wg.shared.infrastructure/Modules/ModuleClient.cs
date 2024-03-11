using wg.shared.abstractions.Modules;
using wg.shared.infrastructure.Modules.Abstractions;

namespace wg.shared.infrastructure.Modules;

internal sealed class ModuleClient(
    IModuleRegistry moduleRegistry,
    IModuleTypesTranslator moduleTypesTranslator) : IModuleClient
{
    public async Task PublishAsync(object message)
    {
        var key = message.GetType().Name;
        var registrations = moduleRegistry.GetBroadcastRegistrations(key);

        var tasks = new List<Task>();

        foreach (var registration in registrations)
        {
            var action = registration.Action;
            var receiverMessage = moduleTypesTranslator.TranslateType(message, registration.ReceiverType);
            tasks.Add(registration.Action(receiverMessage));
        }
        await Task.WhenAll(tasks);
    }
}