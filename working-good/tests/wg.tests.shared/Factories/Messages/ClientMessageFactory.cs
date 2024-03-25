using Bogus;
using wg.modules.messages.core.Entities;

namespace wg.tests.shared.Factories.Messages;

internal static class ClientMessageFactory
{
    internal static ClientMessage Get(bool withNumber)
    {
        Faker<ClientMessage> clientMessageFaker = null;
        if (withNumber)
        {
            var numberFaker = new Faker().Random.Number();
            clientMessageFaker = new Faker<ClientMessage>()
                .CustomInstantiator(f => ClientMessage.Create(
                    $"#{numberFaker}{f.Lorem.Sentence(5)}",
                    f.Lorem.Sentence(10),
                    f.Internet.Email(),
                    f.Date.Recent()));
        }
        else
        {
            clientMessageFaker = new Faker<ClientMessage>()
                .CustomInstantiator(f => ClientMessage.Create(
                    f.Lorem.Sentence(5),
                    f.Lorem.Sentence(10),
                    f.Internet.Email(),
                    f.Date.Recent()));
        }

        var clientMessages = clientMessageFaker.Generate(1);
        return clientMessages.Single();
    }
}