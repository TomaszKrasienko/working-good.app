using wg.shared.abstractions.Events;

namespace wg.modules.companies.application.Events;

public sealed record CompanyAdded(Guid Id, string Name) : IEvent;