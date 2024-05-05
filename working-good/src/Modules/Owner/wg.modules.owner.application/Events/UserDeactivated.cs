using wg.shared.abstractions.Events;

namespace wg.modules.owner.application.Events;

public sealed record UserDeactivated(Guid UserId) : IEvent;