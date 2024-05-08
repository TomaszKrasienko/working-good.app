using Bogus;
using wg.modules.tickets.domain.Entities;

namespace wg.tests.shared.Factories.Tickets;

internal static class MessagesFactory
{
    internal static Message Get()
        => Get(1).Single();
    
    private static List<Message> Get(int count)
         => GetFaker().Generate(count);
    
    private static Faker<Message> GetFaker()
        => new Faker<Message>()
            .CustomInstantiator(f => Message.Create(
                Guid.NewGuid(),
                f.Internet.Email(),
                f.Lorem.Sentence(5),
                f.Lorem.Sentence(10),
                f.Date.Recent()));
}