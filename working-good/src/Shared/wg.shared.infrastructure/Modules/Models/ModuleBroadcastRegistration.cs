namespace wg.shared.infrastructure.Modules.Models;

public class ModuleBroadcastRegistration
{
    public Func<object,Task> Action { get; set; }
}