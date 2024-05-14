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

public sealed class ChangeTicketStatusCommandHandlerTests
{
    private Task Act(ChangeTicketStatusCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenExistingTicket_ShouldUpdateTicketByRepository()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        
        var command = new ChangeTicketStatusCommand(ticket.Id, Status.Cancelled());
        
        //act
        await Act(command);
        
        //assert
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
        
        ticket.Status.Value.ShouldBe(command.State);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingTicket_ShouldThrowTicketNotFoundException()
    {
        //arrange
        var command = new ChangeTicketStatusCommand(Guid.NewGuid(), Status.Cancelled());
        
        //act
        var exception = await Record.ExceptionAsync(async() => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly ICommandHandler<ChangeTicketStatusCommand> _handler;
    
    public ChangeTicketStatusCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _handler = new ChangeTicketStatusCommandHandler(_ticketRepository, TestsClock.Create());
    }
    #endregion
}