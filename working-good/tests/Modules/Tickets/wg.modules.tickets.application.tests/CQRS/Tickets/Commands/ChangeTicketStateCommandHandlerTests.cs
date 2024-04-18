using System.Resources;
using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketState;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class ChangeTicketStateCommandHandlerTests
{
    private Task Act(ChangeTicketStateCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenExistingTicket_ShouldUpdateTicketByRepository()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(1, State.New()).Single();
        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        
        var command = new ChangeTicketStateCommand(ticket.Id, State.Cancelled());
        
        //act
        await Act(command);
        
        //assert
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
        
        ticket.State.Value.ShouldBe(command.State);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingTicket_ShouldThrowTicketNotFoundException()
    {
        //arrange
        var command = new ChangeTicketStateCommand(Guid.NewGuid(), State.Cancelled());
        
        //act
        var exception = await Record.ExceptionAsync(async() => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly ICommandHandler<ChangeTicketStateCommand> _handler;
    
    public ChangeTicketStateCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _handler = new ChangeTicketStateCommandHandler(_ticketRepository, TestsClock.Create());
    }
    #endregion
}