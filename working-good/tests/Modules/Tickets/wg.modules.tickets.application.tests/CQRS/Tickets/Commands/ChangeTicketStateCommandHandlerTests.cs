using NSubstitute;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketState;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class ChangeTicketStateCommandHandlerTests
{
    private Task Act(ChangeTicketStateCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public void HandleAsync_GivenExistingTicket_ShouldUpdateTicketByRepository()
    {
        //
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly ICommandHandler<ChangeTicketStateCommand> _handler;
    
    public ChangeTicketStateCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _handler = new ChangeTicketStateCommandHandler(_ticketRepository);
    }
    #endregion
}