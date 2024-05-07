using System.ServiceModel.Channels;
using Bogus;
using wg.modules.tickets.application.Events.External;

namespace wg.tests.shared.Factories.Events;

internal static class MessageReceivedFactory
{
    internal static MessageReceived Get()
        => Get(1).Single();

    private static List<MessageReceived> Get(int count)
        => GetFaker().Generate(count);
    
    private static Faker<MessageReceived> GetFaker()
         => new Faker<MessageReceived>()
            .CustomInstantiator(f => new MessageReceived(
                f.Internet.Email(),
            f.Lorem.Sentence(5),
            f.Lorem.Sentence(10),
            f.Date.Recent(),
            Guid.NewGuid(),
            10));
}