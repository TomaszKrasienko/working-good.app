using Bogus;
using wg.modules.tickets.domain.Entities;

namespace wg.sharedForTests.Factories.Tickets;

public static class TicketsFactory
{
    public static Ticket GetOnlyRequired(string status)
    {
        var ticketFaker = new Faker<Ticket>()
            .CustomInstantiator(f => Ticket.Create(
                Guid.NewGuid(),
                new Random().Next(1, 2000),
                f.Lorem.Sentence(),
                f.Lorem.Sentence(),
                DateTime.Now,
                Guid.NewGuid(),
                status,
                DateTime.Now,
                false));
        var ticket = ticketFaker.Generate(1).Single();
        return ticket;
    }
}