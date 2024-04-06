using wg.shared.abstractions.Events;

namespace wg.modules.companies.application.Events;

public record ProjectEdited(Guid Id, string Title) : IEvent;