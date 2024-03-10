using wg.shared.abstractions.Events;

namespace wg.modules.companies.application.Events;

public record ProjectAdded(Guid Id, string Title) : IEvent;