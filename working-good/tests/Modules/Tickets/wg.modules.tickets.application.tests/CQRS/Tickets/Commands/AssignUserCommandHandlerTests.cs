using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignUser;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class AssignUserCommandHandlerTests
{
    private Task Act(AssignUserCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenNotExistingTicket_ShouldThrowTicketNotFoundException()
    {
        //arrange
        var command = new AssignUserCommand(Guid.NewGuid(), Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingUser_ShouldThrowUserNotFoundException()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired().Single();
        var command = new AssignUserCommand(Guid.NewGuid(), ticket.Id);

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<UserNotFoundException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenProjectIdAndNoExistingUserInProject_ShouldUserDoesNotBelongToGroupException()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired().Single();
        //Todo: Dodać assign projectId
        var command = new AssignUserCommand(Guid.NewGuid(), ticket.Id);

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<UserNotFoundException>();
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly AssignUserCommandHandler _handler;

    public AssignUserCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _handler = new AssignUserCommandHandler(_ticketRepository, _ownerApiClient);
    }

    #endregion
}