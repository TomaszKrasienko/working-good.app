using wg.shared.abstractions.Events;

namespace wg.modules.tickets.application.Events.External;

public sealed record UserDeactivated(Guid UserId) : IEvent;