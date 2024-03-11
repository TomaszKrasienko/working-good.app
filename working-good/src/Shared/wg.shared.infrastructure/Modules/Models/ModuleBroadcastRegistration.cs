using Microsoft.AspNetCore.Identity;

namespace wg.shared.infrastructure.Modules.Models;

public class ModuleBroadcastRegistration(Type receiverType, Func<object, Task> action)
{
    public Type ReceiverType { get; } = receiverType;
    public Func<object,Task> Action { get; } = action;
}