using Bogus;
using Bogus.DataSets;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.ValueObjects.Ticket;

namespace wg.tests.shared.Factories.Tickets;

public static class TicketsFactory
{
    internal static Ticket Get()
        => Get(1).Single();

    internal static List<Ticket> Get(int count)
        => GetFaker().Generate(count);

    private static Faker<Ticket> GetFaker()
        => new Faker<Ticket>().CustomInstantiator(v
            => Ticket.Create(
                Guid.NewGuid(),
                v.Random.Number(1000),
                v.Lorem.Sentence(null, 5),
                v.Lorem.Sentence(null, 10),
                v.Date.Recent(),
                v.Internet.Email()
            ));
}