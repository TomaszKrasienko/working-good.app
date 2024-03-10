using Bogus;
using wg.shared.abstractions.Events;
using wg.shared.abstractions.Messaging;
using wg.sharedForTests.Models;

namespace wg.sharedForTests.Factories.Events;

public static class TestEventsFactory
{
    public static IMessage[] Get()
    {
        var eventFaker = new Faker<TestEvent>()
            .CustomInstantiator(f => new TestEvent(Guid.NewGuid()));
        Random random = new Random();
        int count = random.Next(1, 5);
        return eventFaker.Generate(count).ToArray();
    }
}