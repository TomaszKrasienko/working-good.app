using Bogus;
using wg.modules.tickets.domain.Entities;

namespace wg.sharedForTests.Factories.Tickets;

public static class TicketsFactory
{
    public static Ticket GetOnlyRequired(string state)
    {
        var ticketFaker = new Faker<Ticket>()
            .CustomInstantiator(f => Ticket.Create(
                Guid.NewGuid(),
                new Random().Next(1, 2000),
                f.Lorem.Sentence(),
                f.Lorem.Sentence(),
                DateTime.Now,
                Guid.NewGuid(),
                state,
                DateTime.Now,
                false));
        var ticket = ticketFaker.Generate(1).Single();
        return ticket;
    }

    public static Ticket GetAll(string state)
    {
        var ticketFaker = new Faker<Ticket>()
            .CustomInstantiator(f => Ticket.Create(
                Guid.NewGuid(),
                new Random().Next(1, 2000),
                f.Lorem.Sentence(),
                f.Lorem.Sentence(),
                DateTime.Now,
                Guid.NewGuid(),
                state,
                DateTime.Now,
                false,
                f.Date.Future(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()));
        
        var ticket = ticketFaker.Generate(1).Single();
        return ticket;
    }
}