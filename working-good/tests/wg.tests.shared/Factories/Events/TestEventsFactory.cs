using Bogus;
using wg.shared.abstractions.Events;
using wg.shared.abstractions.Messaging;
using wg.tests.shared.Models;

namespace wg.tests.shared.Factories.Events;

internal static class TestEventsFactory
{
    internal static TestEvent[] Get()
    {
        Random random = new Random();
        int count = random.Next(1, 5);
        return GetFaker().Generate(count).ToArray();
    }
    
    private static Faker<TestEvent> GetFaker()
        => new Faker<TestEvent>()
            .CustomInstantiator(f => new TestEvent(Guid.NewGuid()));
}