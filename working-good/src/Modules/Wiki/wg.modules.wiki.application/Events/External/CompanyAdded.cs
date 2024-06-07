using wg.shared.abstractions.Events;

namespace wg.modules.wiki.application.Events.External;

public sealed record CompanyAdded(Guid Id, string Name) : IEvent;