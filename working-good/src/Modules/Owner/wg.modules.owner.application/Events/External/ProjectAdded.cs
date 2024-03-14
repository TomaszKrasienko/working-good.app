using wg.shared.abstractions.Events;

namespace wg.modules.owner.application.Events.External;

public record ProjectAdded(Guid Id, string Title) : IEvent;