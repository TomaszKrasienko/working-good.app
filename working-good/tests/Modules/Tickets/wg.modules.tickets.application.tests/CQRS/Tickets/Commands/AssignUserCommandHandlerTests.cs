using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Shouldly;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignUser;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;
using wg.tests.shared.Factories.DTOs.Tickets.Owner;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class AssignUserCommandHandlerTests
{
    private Task Act(AssignUserCommand command) => _handler.HandleAsync(command, default);    

    [Fact]
    public async Task HandleAsync_GivenValidArguments_ShouldUpdateTicketAndSentEvent()
    {
      //arrange
      var ticket = TicketsFactory.Get();
      var command = new AssignUserCommand(Guid.NewGuid(), ticket.Id);
      var projectId = Guid.NewGuid();
      ticket.ChangeProject(projectId);

      _ticketRepository
          .GetByIdAsync(ticket.Id)
          .Returns(ticket);

      _ownerApiClient
          .IsMembershipExistsAsync(Arg.Is<GetMembershipDto>(arg
              => arg.UserId == command.UserId
                 && arg.GroupId == projectId))
          .Returns(new IsGroupMembershipExists()
          {
              Value = true
          });
      
      //act
      await Act(command);
      
      //assert
      ticket.AssignedUser.Value.ShouldBe(command.UserId);

      await _ticketRepository
          .Received(1)
          .UpdateAsync(ticket);

      await _messageBroker
          .PublishAsync(Arg.Is<UserAssigned>(arg
              => arg.TicketId == command.TicketId
                 && arg.TicketNumber == ticket.Number.Value
                 && arg.UserId == command.UserId));
    }

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
    public async Task HandleAsync_GivenTicketForProjectAndUserNotBelongToProject_ShouldThrowUserDoesNotBelongToGroupException()
    {
      //arrange
      var ticket = TicketsFactory.Get();
      var command = new AssignUserCommand(Guid.NewGuid(), ticket.Id);
      var projectId = Guid.NewGuid();
      ticket.ChangeProject(projectId);

      _ticketRepository
          .GetByIdAsync(ticket.Id)
          .Returns(ticket);

      _ownerApiClient
          .IsMembershipExistsAsync(Arg.Is<GetMembershipDto>(arg
              => arg.UserId == command.UserId
                 && arg.GroupId == projectId))
          .Returns(new IsGroupMembershipExists()
          {
              Value = false
          });
      
      //act
      var exception = await Record.ExceptionAsync(async () => await Act(command));
      
      //assert
      exception.ShouldBeOfType<UserDoesNotBelongToGroupException>();
    }
     
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly IMessageBroker _messageBroker;
    private readonly AssignUserCommandHandler _handler;
    
    public AssignUserCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new AssignUserCommandHandler(_ticketRepository, _ownerApiClient, _messageBroker);
    }
    #endregion
}