using Shouldly;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.domain.tests.Entities;

public sealed class TicketTests
{
    [Fact]
    public void AddMessage_GivenMessageAnd_ShouldAddToMessages()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(State.New());
        var id = Guid.NewGuid();
        var sender = "joe@doe.pl";
        var subject = "Test subject";
        var content = "Test content";
        var createdAt = DateTime.Now;
        
        //act
        ticket.AddMessage(id, sender, subject, content, createdAt);
        
        //assert
        var message = ticket.Messages.FirstOrDefault(x => x.Id.Equals(id));
        message.ShouldNotBeNull();
        message.Id.Value.ShouldBe(id);
        message.Sender.Value.ShouldBe(sender);
        message.Subject.Value.ShouldBe(subject);
        message.Content.Value.ShouldBe(content);
        message.CreatedAt.Value.ShouldBe(createdAt);
    }
}