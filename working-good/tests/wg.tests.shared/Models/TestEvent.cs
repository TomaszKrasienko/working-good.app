using wg.shared.abstractions.Events;

namespace wg.tests.shared.Models;

public sealed record TestEvent(Guid Id) : IEvent;