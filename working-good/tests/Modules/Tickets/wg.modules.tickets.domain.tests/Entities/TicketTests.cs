using Shouldly;
using wg.modules.tickets.domain.Exceptions;
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
        var ticket = TicketsFactory.GetOnlyRequired(state: State.New()).Single();
        var id = Guid.NewGuid();
        var sender = "joe@doe.pl";
        var subject = "Test subject";
        var content = "Test content";
        var createdAt = DateTime.Now;
        
        //act
        ticket.AddMessage(id, sender, subject, content, createdAt);
        
        //assert
        var message = ticket.Messages.FirstOrDefault(x => x.Id.Equals(id));
        ticket.State.Value.ShouldBe(State.Open());
        message.ShouldNotBeNull();
        message.Id.Value.ShouldBe(id);
        message.Sender.Value.ShouldBe(sender);
        message.Subject.Value.ShouldBe(subject);
        message.Content.Value.ShouldBe(content);
        message.CreatedAt.Value.ShouldBe(createdAt);
    }

    [Fact]
    public void ChangeAssignedUser_GivenUserIdAndDateForStateNew_ShouldChangeAssignedUserAndStateToOpenAndStateDate()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.New()).Single();
        var userId = Guid.NewGuid();
        var date = DateTime.Now;
        
        //act
        ticket.ChangeAssignedUser(userId, date);
        
        //assert
        ticket.AssignedUser.Value.ShouldBe(userId);
        ticket.State.Value.ShouldBe(State.Open());
        ticket.State.ChangeDate.ShouldBe(date);
    }
    
    [Fact]
    public void ChangeAssignedUser_GivenUserIdAndDateForStateInProgress_ShouldChangeAssignedUserAndNotChangeState()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.InProgress()).Single();
        var currentStateDate = ticket.State.ChangeDate;
        var userId = Guid.NewGuid();
        var date = DateTime.Now;
        
        //act
        ticket.ChangeAssignedUser(userId, date);
        
        //assert
        ticket.AssignedUser.Value.ShouldBe(userId);
        ticket.State.Value.ShouldBe(State.InProgress());
        ticket.State.ChangeDate.ShouldBe(currentStateDate);
    }

    [Fact]
    public void ChangeAssignedUser_ForCancelledState_ShouldThrowInvalidStateForAssignUserException()
    {        
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.Cancelled()).Single();
        var userId = Guid.NewGuid();
        var date = DateTime.Now;
        
        //act
        var exception = Record.Exception(() => ticket.ChangeAssignedUser(userId, date));
        
        //assert
        exception.ShouldBeOfType<InvalidStateForAssignUserException>();
    }
}