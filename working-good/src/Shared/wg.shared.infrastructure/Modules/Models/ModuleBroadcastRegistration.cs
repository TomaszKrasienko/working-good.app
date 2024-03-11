using Microsoft.AspNetCore.Identity;

namespace wg.shared.infrastructure.Modules.Models;

public class ModuleBroadcastRegistration
{
    public Type ReceiverType { get; set; }
    public Func<object,Task> Action { get; set; }
}