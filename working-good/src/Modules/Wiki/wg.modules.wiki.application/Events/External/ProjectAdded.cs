using wg.shared.abstractions.Events;

namespace wg.modules.wiki.application.Events.External;

public record ProjectAdded(Guid Id, string Title) : IEvent;