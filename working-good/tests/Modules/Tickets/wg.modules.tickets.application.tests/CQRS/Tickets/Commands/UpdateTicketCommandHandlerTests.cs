using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.CQRS.Tickets.Commands.UpdateTicket;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class UpdateTicketCommandHandlerTests
{
    private Task Act(UpdateTicketCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingTicketId_ShouldUpdateTicketAndUpdateByRepository()
    {
        //arrange
        var ticket = TicketsFactory.Get();

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        var command = new UpdateTicketCommand(Guid.NewGuid(), "new_subject", "new_content");
        
        //act
        await Act(command);
        
        //assert
        ticket.Subject.Value.ShouldBe(command.Subject);
        ticket.Content.Value.ShouldBe(command.Content);

        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingTicketId_ShouldThrowTicketNotFoundException()
    {
        //arrange
        var command = new UpdateTicketCommand(Guid.NewGuid(), "Subject", "Content");
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly ICommandHandler<UpdateTicketCommand> _handler;
    
    public UpdateTicketCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _handler = new UpdateTicketCommandHandler(_ticketRepository);
    }
    #endregion
}