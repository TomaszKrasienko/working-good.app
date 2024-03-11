using wg.shared.infrastructure.Modules.Models;

namespace wg.shared.infrastructure.Modules.Abstractions;

public interface IModuleRegistry
{
    IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string key);
}