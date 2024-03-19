using wg.shared.infrastructure.Modules.Models;

namespace wg.shared.infrastructure.Modules.Abstractions;

internal interface IModuleRegistry
{
    IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string key);
    void AddBroadcastingRegistration(Type requestType, Func<object, Task> action);
    ModuleRequestRegistration GetRequestRegistration(string path);
    void AddRequestRegistration(string path, Type requestType, Type responseType, Func<object, Task<object>> action);
}