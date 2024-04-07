using wg.shared.abstractions.Events;

namespace wg.modules.owner.application.Events.External;

public record ProjectEdited(Guid Id, string Title) : IEvent;