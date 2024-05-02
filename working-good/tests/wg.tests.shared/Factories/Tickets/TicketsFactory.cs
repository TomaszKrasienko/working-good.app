using Bogus;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.ValueObjects.Ticket;

namespace wg.tests.shared.Factories.Tickets;

public static class TicketsFactory
{
    public static List<Ticket> GetOnlyRequired(int count = 1, string state = null)
    {
        var states = State.AvailableStates;
        var ticketFaker = new Faker<Ticket>()
            .CustomInstantiator(f => Ticket.Create(
                Guid.NewGuid(),
                ++f.IndexFaker,
                f.Lorem.Sentence(),
                f.Lorem.Sentence(),
                DateTime.Now,
                f.Internet.Email(),
                state ?? f.PickRandom(states),
                DateTime.Now,
                false));
        var ticket = ticketFaker.Generate(count);
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
                f.Internet.Email(),
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