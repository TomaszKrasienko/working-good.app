using Bogus;
using wg.modules.notifications.core.Events.External;
using wg.modules.tickets.domain.Entities;

namespace wg.tests.shared.Factories.Events;

internal static class TicketCreatedFactory
{
    internal static TicketCreated Get(bool withUser = false, bool withEmployee = false)
        => Get(1, withUser, withEmployee).Single();
    
    private static List<TicketCreated> Get(int count, bool withUser, bool withEmployee)
        => GetFaker(withUser, withEmployee).Generate(1);
            
    private static Faker<TicketCreated> GetFaker(bool withUser, bool withEmployee)
        =>  new Faker<TicketCreated>()
            .CustomInstantiator(f => new TicketCreated(
            Guid.NewGuid(),
            f.Random.Number(),
            f.Lorem.Sentence(4),
            f.Lorem.Sentence(10),
            withUser ? Guid.NewGuid() : null,
            withEmployee ? Guid.NewGuid() : null));
}