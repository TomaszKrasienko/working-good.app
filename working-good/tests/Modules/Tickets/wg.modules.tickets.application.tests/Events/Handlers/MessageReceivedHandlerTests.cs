using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Events.External;
using wg.modules.tickets.application.Events.External.Handlers;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.Events.Handlers;

public sealed class MessageReceivedHandlerTests
{
    private Task Act(MessageReceived @event) => _handler.HandleAsync(@event);

    [Fact]
    public async Task HandleAsync_GivenExistingTicketNumber_ShouldAddNewMessageAndEmailToTicketAndChangeStatus()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(State.Done());
        var @event = new MessageReceived("joe.doe@test.pl", "Some problems with app", "I have some problems with app",
            DateTime.Now, Guid.NewGuid(), ticket.Number);

        _ticketRepository
            .GetByNumberAsync((int)@event.TicketNumber!)
            .Returns(ticket);
        
        //act
        await Act(@event);
        
        //assert
        var message = ticket.Messages.SingleOrDefault();
        message.ShouldNotBeNull();
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNullTicketNumber_ShouldAddNewTicket()
    {
        //arrange
        var @event = new MessageReceived("joe.doe@test.pl", "Some problems with app", "I have some problems with app",
            DateTime.Now, Guid.NewGuid(), null);
        var number = 1;
        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(number);
        
        //act
        await Act(@event);
        
        //assert
        await _ticketRepository
            .Received(1)
            .AddAsync(Arg.Is<Ticket>(arg
                => arg.Number == number + 1
                   && arg.Subject == @event.Subject
                   && arg.Content == @event.Content
                   && arg.CreatedAt.Equals(@event.CreatedAt)
                   && arg.CreatedBy.Equals(Guid.Empty)
                   && arg.AssignedEmployee.Equals(@event.AssignedEmployee)
                   && arg.Emails.Contains(@event.Sender)));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingTicketNumber_ShouldThrowTicketNumberNotFoundException()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(State.Done());
        var @event = new MessageReceived("joe.doe@test.pl", "Some problems with app", "I have some problems with app",
            DateTime.Now, Guid.NewGuid(), ticket.Number);

        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(@event));
        
        //assert
        await _ticketRepository
            .Received(0)
            .UpdateAsync(ticket);
        
        exception.ShouldBeOfType<TicketNumberNotFoundException>();
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly MessageReceivedHandler _handler;

    public MessageReceivedHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _handler = new MessageReceivedHandler(_ticketRepository);
    }
    #endregion
}