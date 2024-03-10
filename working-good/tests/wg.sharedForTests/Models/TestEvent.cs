using wg.shared.abstractions.Events;

namespace wg.sharedForTests.Models;

public sealed record TestEvent(Guid Id) : IEvent;