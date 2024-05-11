using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignProject;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class AssignProjectCommandHandlerTests
{
    private Task Act(AssignProjectCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenExistingTicketWithAssignedUser_ShouldAssignProjectToTicket()
    {
        //arrange
        var userId = Guid.NewGuid();
        var ticket = TicketsFactory.GetOnlyRequired(State.Open());
        ticket.ChangeAssignedUser(userId, DateTime.Now);
        
        var membershipDto = new IsGroupMembershipExists()
        {
            Value = true
        };

        var command = new AssignProjectCommand(Guid.NewGuid(), Guid.NewGuid());
        
        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        _ownerApiClient
            .IsMembershipExistsAsync(Arg.Is<GetMembershipDto>(arg
                => arg.GroupId == command.ProjectId 
                && arg.UserId == userId))
            .Returns(membershipDto);
        
        //act
        await Act(command);
        
        //arrange
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
        
        ticket.ProjectId.Value.ShouldBe(command.ProjectId);
    }
    
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly ICommandHandler<AssignProjectCommand> _handler;
    
    public AssignProjectCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _handler = new AssignProjectCommandHandler(_ticketRepository, _ownerApiClient);
    }

    #endregion
}