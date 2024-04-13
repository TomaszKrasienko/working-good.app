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
using wg.tests.shared.Factories.DTOs.Tickets;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class AssignUserCommandHandlerTests
{
     private Task Act(AssignUserCommand command) => _handler.HandleAsync(command, default);

     [Fact]
     public async Task HandleAsync_GivenExistingTicketAndUserInProject_ShouldUpdateTicketByRepository()
     {
         //arrange
         var userDto = new UserDto()
         {
             Id = Guid.NewGuid(),
             Email = "joe.doe@user.pl",
             FirstName = "Joe",
             LastName = "Doe",
             Role = "Manager",
             State = "active"
         };

         var groupDto = new GroupDto()
         {
             Id = Guid.NewGuid(),
             Title = "Group test title",
             Users = [userDto.Id],
         };

         var ownerDto = new OwnerDto()
         {
             Id = Guid.NewGuid(),
             Name = "Owner name",
             Groups = [groupDto],
             Users = [userDto]
         };

         _ownerApiClient
             .GetOwnerAsync()
             .Returns(ownerDto);
         
         var ticket = TicketsFactory.GetOnlyRequired(state:State.Open()).Single();
         ticket.ChangeProject(groupDto.Id);

         _ticketRepository
             .GetByIdAsync(ticket.Id)
             .Returns(ticket);
         
         var command = new AssignUserCommand(userDto.Id, ticket.Id);
         //act
         await Act(command);
         
         //assert
         await _ticketRepository
             .Received(1)
             .UpdateAsync(ticket);

         await _ownerApiClient
             .Received(0)
             .GetActiveUserByIdAsync(Arg.Any<UserIdDto>());
     }
     
     [Fact]
     public async Task HandleAsync_GivenExistingTicketAndWithoutProjectId_ShouldUpdateTicketByRepositoryAndNotCheckUserInGroup()
     {
         //arrange
         var userDto = new UserDto()
         {
             Id = Guid.NewGuid(),
             Email = "joe.doe@user.pl",
             FirstName = "Joe",
             LastName = "Doe",
             Role = "Manager",
             State = "active"
         };

         _ownerApiClient
             .GetActiveUserByIdAsync(Arg.Is<UserIdDto>(arg => arg.Id == userDto.Id))
             .Returns(userDto);
         
         var ticket = TicketsFactory.GetOnlyRequired(state:State.Open()).Single();

         _ticketRepository
             .GetByIdAsync(ticket.Id)
             .Returns(ticket);
         
         var command = new AssignUserCommand(userDto.Id, ticket.Id);
         
         //act
         await Act(command);
         
         //assert
         await _ticketRepository
             .Received(1)
             .UpdateAsync(ticket);

         await _ownerApiClient
             .Received(0)
             .GetOwnerAsync();
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
//arrange
         var userDto = new UserDto()
         {
             Id = Guid.NewGuid(),
             Email = "joe.doe@user.pl",
             FirstName = "Joe",
             LastName = "Doe",
             Role = "Manager",
             State = "active"
         };

         var groupDto = new GroupDto()
         {
             Id = Guid.NewGuid(),
             Title = "Group test title",
             Users = [],
         };

         var ownerDto = new OwnerDto()
         {
             Id = Guid.NewGuid(),
             Name = "Owner name",
             Groups = [groupDto],
             Users = [userDto]
         };

         _ownerApiClient
             .GetOwnerAsync()
             .Returns(ownerDto);
         
         var ticket = TicketsFactory.GetOnlyRequired(state:State.Open()).Single();
         ticket.ChangeProject(groupDto.Id);

         _ticketRepository
             .GetByIdAsync(ticket.Id)
             .Returns(ticket);
         
         var command = new AssignUserCommand(userDto.Id, ticket.Id);
         
         //act
         var exception = await Record.ExceptionAsync(async () => await Act(command));
         
         //assert
         exception.ShouldBeOfType<UserDoesNotBelongToGroupException>();
     }
     
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