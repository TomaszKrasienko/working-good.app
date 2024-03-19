using wg.shared.infrastructure.Modules.Abstractions;
using wg.shared.infrastructure.Modules.Models;

namespace wg.shared.infrastructure.Modules;

internal sealed class ModuleRegistry : IModuleRegistry
{
    private readonly List<ModuleBroadcastRegistration> _broadcastRegistrations = new();
    private readonly Dictionary<string, ModuleRequestRegistration> _requestRegistrations = new();

    public IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string key)
        => _broadcastRegistrations.Where(x => x.Key == key);

    public void AddBroadcastingRegistration(Type requestType, Func<object, Task> action)
    {
        if (string.IsNullOrWhiteSpace(requestType.Namespace))
        {
            throw new InvalidOperationException("Missing namespace");
        }

        var registration = new ModuleBroadcastRegistration(requestType, action);
        _broadcastRegistrations.Add(registration);
    }

    public ModuleRequestRegistration GetRequestRegistration(string path)
        => _requestRegistrations.TryGetValue(path, out var registration) ? registration : null;

    public void AddRequestRegistration(string path, Type requestType, Type responseType, Func<object, Task<object>> action)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new InvalidOperationException("Path can not be null or empty");
        }

        var registration = new ModuleRequestRegistration(requestType, responseType, action);
        _requestRegistrations.Add(path, registration);
    }
}