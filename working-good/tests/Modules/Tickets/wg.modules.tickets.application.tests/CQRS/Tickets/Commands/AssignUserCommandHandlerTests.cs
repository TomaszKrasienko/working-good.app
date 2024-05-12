using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignUser;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
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
      
//
//      [Fact]
//      public async Task HandleAsync_GivenExistingTicketAndUserInProject_ShouldUpdateTicketByRepository()
//      {
//          //arrange
//          var userDto = UserDtoFactory.Get();
//          var groupDto = GroupDtoFactory.Get();
//          var ownerDto = OwnerDtoFactory.Get();
//          groupDto.Users = [userDto.Id];
//          ownerDto.Users = [userDto];
//          ownerDto.Groups = [groupDto];
//
//          _ownerApiClient
//              .GetOwnerAsync(Arg.Any<GetOwnerDto>())
//              .Returns(ownerDto);
//          
//          var ticket = TicketsFactory.GetOnlyRequired(state:State.Open());
//          ticket.ChangeProject(groupDto.Id);
//
//          _ticketRepository
//              .GetByIdAsync(ticket.Id)
//              .Returns(ticket);
//          
//          var command = new AssignUserCommand(userDto.Id, ticket.Id);
//          
//          //act
//          await Act(command);
//          
//          //assert
//          await _ticketRepository
//              .Received(1)
//              .UpdateAsync(ticket);
//
//          await _ownerApiClient
//              .Received(0)
//              .GetActiveUserByIdAsync(Arg.Any<UserIdDto>());
//      }
//      
//      [Fact]
//      public async Task HandleAsync_GivenExistingTicketAndWithoutProjectId_ShouldUpdateTicketByRepositoryAndNotCheckUserInGroup()
//      {
//          //arrange
//          var userDto = UserDtoFactory.Get();
//
//          _ownerApiClient
//              .GetActiveUserByIdAsync(Arg.Is<UserIdDto>(arg => arg.Id == userDto.Id))
//              .Returns(userDto);
//          
//          var ticket = TicketsFactory.GetOnlyRequired(State.Open());
//
//          _ticketRepository
//              .GetByIdAsync(ticket.Id)
//              .Returns(ticket);
//          
//          var command = new AssignUserCommand(userDto.Id, ticket.Id);
//          
//          //act
//          await Act(command);
//          
//          //assert
//          await _ticketRepository
//              .Received(1)
//              .UpdateAsync(ticket);
//
//          await _ownerApiClient
//              .Received(0)
//              .GetOwnerAsync(Arg.Any<GetOwnerDto>());
//      }
//      

//      
//      [Fact]
//      public async Task HandleAsync_GivenNotExistingUser_ShouldThrowUserNotFoundException()
//      {
//          //arrange
//          var ticket = TicketsFactory.GetOnlyRequired();
//          var command = new AssignUserCommand(Guid.NewGuid(), ticket.Id);
//
//          _ticketRepository
//              .GetByIdAsync(ticket.Id)
//              .Returns(ticket);
//          
//          //act
//          var exception = await Record.ExceptionAsync(async () => await Act(command));
//          
//          //assert
//          exception.ShouldBeOfType<UserNotFoundException>();
//      }
//      
//      [Fact]
//      public async Task HandleAsync_GivenProjectIdAndNoExistingUserInProject_ShouldUserDoesNotBelongToGroupException()
//      {
//          //arrange
//          var userDto = UserDtoFactory.Get();
//          var groupDto = GroupDtoFactory.Get();
//          var ownerDto = OwnerDtoFactory.Get();
//          ownerDto.Users = [userDto];
//          ownerDto.Groups = [groupDto];
//
//          _ownerApiClient
//              .GetOwnerAsync(Arg.Any<GetOwnerDto>())
//              .Returns(ownerDto);
//          
//          var ticket = TicketsFactory.GetOnlyRequired(state:State.Open());
//          ticket.ChangeProject(groupDto.Id);
//
//          _ticketRepository
//              .GetByIdAsync(ticket.Id)
//              .Returns(ticket);
//          
//          var command = new AssignUserCommand(userDto.Id, ticket.Id);
//          
//          //act
//          var exception = await Record.ExceptionAsync(async () => await Act(command));
//          
//          //assert
//          exception.ShouldBeOfType<UserDoesNotBelongToGroupException>();
//      }
     
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly AssignUserCommandHandler _handler;
    private readonly IClock _clock;
    
    public AssignUserCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _clock = TestsClock.Create();
        _handler = new AssignUserCommandHandler(_ticketRepository, _ownerApiClient, _clock);
    }
    #endregion
}