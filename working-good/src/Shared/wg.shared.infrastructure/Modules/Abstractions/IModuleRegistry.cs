using wg.shared.infrastructure.Modules.Models;

namespace wg.shared.infrastructure.Modules.Abstractions;

internal interface IModuleRegistry
{
    IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string key);
    void AddBroadcastingRegistration(Type requestType, Func<object, Task> action);
}