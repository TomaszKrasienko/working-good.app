using Bogus;
using wg.modules.notifications.core.Events.External;

namespace wg.tests.shared.Factories.Events;

internal static class TicketCreatedFactory
{
    internal static TicketCreated Get(bool withUser = false, bool withEmployee = false)
    {
        var ticketCreatedFaker = new Faker<TicketCreated>()
            .CustomInstantiator(f => new TicketCreated(
                Guid.NewGuid(),
                f.Random.Number(),
                f.Lorem.Sentence(4),
                f.Lorem.Sentence(10),
                withUser ? Guid.NewGuid() : null,
                withEmployee ? Guid.NewGuid() : null));

        return ticketCreatedFaker.Generate(1).Single();
    }
}