using Bogus;
using wg.modules.tickets.application.Events.External;

namespace wg.tests.shared.Factories.Events;

internal static class MessageReceivedFactory
{
    internal static MessageReceived Get()
    {
        var faker = new Faker<MessageReceived>()
            .RuleFor(f => f.Sender, v => v.Internet.Email())
            .RuleFor(f => f.Subject, v => v.Lorem.Sentence(5))
            .RuleFor(f => f.Content, v => v.Lorem.Sentence(10))
            .RuleFor(f => f.CreatedAt, v => v.Date.Recent())
            .RuleFor(f => f.TicketNumber, v => 10);
        return faker.Generate(1).Single();
    }
}