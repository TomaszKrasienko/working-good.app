using Bogus;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.ValueObjects.Ticket;

namespace wg.tests.shared.Factories.Tickets;

public static class TicketsFactory
{
    internal static Ticket GetOnlyRequired(string state = null)
        => Get(1, false, state).Single();
    
    internal static Ticket GetAll(string state = null)
        => Get(1, true, state).Single();

    internal static List<Ticket> Get(int count, bool all, string state = null)
    {
        var tickets = GetFaker(all).Generate(count);
        foreach (var ticket in tickets)
        {
            if (string.IsNullOrEmpty(state))
            {
                var index = new Random().Next(State.AvailableStates.Count);
                ticket.ChangeState(State.AvailableStates.Keys.ToList()[index], DateTime.Now);
            }
            else
            {
                ticket.ChangeState(state, DateTime.Now);
            }
        }

        return tickets;
    }

    private static Faker<Ticket> GetFaker(bool all)
        => all 
            ? new Faker<Ticket>()
                .CustomInstantiator(f => Ticket.Create(
                    Guid.NewGuid(),
                    new Random().Next(1, 2000),
                    f.Lorem.Sentence(),
                    f.Lorem.Sentence(),
                    DateTime.Now,
                    f.Internet.Email(),
                    State.New(),
                    DateTime.Now,
                    false,
                    f.Date.Future(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()))
            : new Faker<Ticket>()
                .CustomInstantiator(f => Ticket.Create(
                    Guid.NewGuid(),
                    ++f.IndexFaker,
                    f.Lorem.Sentence(),
                    f.Lorem.Sentence(),
                    DateTime.Now,
                    f.Internet.Email(),
                    State.New(),
                    DateTime.Now,
                    false));
}