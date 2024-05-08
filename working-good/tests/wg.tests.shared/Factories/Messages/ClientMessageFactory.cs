using Bogus;
using wg.modules.messages.core.Models;

namespace wg.tests.shared.Factories.Messages;

internal static class ClientMessageFactory
{
    internal static ClientMessage Get(bool withNumber)
        => Get(1, withNumber).Single();

    private static List<ClientMessage> Get(int count, bool withNumber)
        => GetFaker(withNumber).Generate(count);
    
    private static Faker<ClientMessage> GetFaker(bool withNumber)
        => new Faker<ClientMessage>()
            .CustomInstantiator(f => ClientMessage.Create(
                withNumber ? $"#{GetRandomNumber()}{f.Lorem.Sentence(5)}" : f.Lorem.Sentence(5),
                f.Lorem.Sentence(10),
                f.Internet.Email(),
                f.Date.Recent(),
                Guid.NewGuid()));
    
    private static int GetRandomNumber()
        => new Faker().Random.Number();
}