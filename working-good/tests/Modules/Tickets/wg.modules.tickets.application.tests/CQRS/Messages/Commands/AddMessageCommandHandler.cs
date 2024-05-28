using NSubstitute;
using Shouldly;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Messages.Commands.AddMessage;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Messages.Commands;

public sealed class AddMessageCommandHandlerTests
{
    private Task Act(AddMessageCommand command) => _handler.HandleAsync(command, default);
    
    // [Fact]
    // public async Task HandleAsync_GivenExistingTicket_ShouldUpdateTicketWithNewMessageAndSendEvent()
    // {
//         //arrange
//         var ticket = TicketsFactory.Get();
//         var command = new AddMessageCommand(Guid.NewGuid(), Guid.NewGuid(), "Test message contet",
//             ticket.Id);
//
//         _ticketRepository
//             .GetByIdAsync(command.TicketId)
//             .Returns(ticket);
//
//         var userDto = new UserDto()
//         {
//             Id = command.UserId,
//             Email = "test@tst.pl",
//             FirstName = "Joe",
//             LastName = "Doe",
//             Role = Role.User()
//         };
//
//         _ownerApiClient
//             .GetActiveUserByIdAsync(new UserIdDto(command.UserId))
//             .Returns(userDto);
//         
//         //act
//         await Act(command);
//         
//         //assert
//         ticket.State.Value.ShouldBe(State.WaitingForResponse());
//         
//         await _ticketRepository
//             .Received(1)
//             .UpdateAsync(Arg.Is<Ticket>(arg
//                 => arg.Id == ticket.Id
//                    && arg.State.Value == State.WaitingForResponse()
//                    && arg.Messages.Any(m 
//                        => m.Content == command.Content
//                        && m.Sender == userDto.Email
//                        && m.CreatedAt.Value == _now
//                        )
//             ));
//
//         await _messageBroker
//             .Received(1)
//             .PublishAsync(Arg.Is<MessageAdded>(arg
//                 => arg.TicketNumber == ticket.Number
//                    && arg.Subject == ticket.Subject
//                    && arg.Content == command.Content
//                    && arg.EmployeeId == ticket.AssignedEmployee.Value));
//     }
//
//     [Fact]
//     public async Task HandleAsync_GivenExistingTicketWithoutAssignedEmployee_ShouldUpdateTicketWithNewMessageAndSendEvent()
//     {
//         //arrange
//         var ticket = TicketsFactory.Get();
//         var command = new AddMessageCommand(Guid.NewGuid(), Guid.NewGuid(), "Test message contet",
//             ticket.Id);
//         
//         _ticketRepository
//             .GetByIdAsync(command.TicketId)
//             .Returns(ticket);
//         
//         var userDto = new UserDto()
//         {
//             Id = command.UserId,
//             Email = "test@tst.pl",
//             FirstName = "Joe",
//             LastName = "Doe",
//             Role = Role.User()
//         };
//
//         _ownerApiClient
//             .GetActiveUserByIdAsync(new UserIdDto(command.UserId))
//             .Returns(userDto);
//         
//         //act
//         await Act(command);
//         
//         //assert
//         await _ticketRepository
//             .Received(1)
//             .UpdateAsync(Arg.Is<Ticket>(arg
//                 => arg.Id == ticket.Id
//                    && arg.State.Value == State.WaitingForResponse()
//                    && arg.Messages.Any(m 
//                        => m.Content == command.Content
//                           && m.Sender == userDto.Email
//                           && m.CreatedAt.Value == _now
//                          )));
//
//         await _messageBroker
//             .Received(1)
//             .PublishAsync(Arg.Is<MessageAdded>(arg
//                 => arg.TicketNumber == ticket.Number
//                    && arg.Subject == ticket.Subject
//                    && arg.Content == command.Content
//                    && arg.EmployeeId == null));
//     }
//     
     [Fact]
     public async Task HandleAsync_GivenNotExistingTicket_ShouldUThrowTicketNotFoundException()
     {
         //arrange
         var command = new AddMessageCommand(Guid.NewGuid(), Guid.NewGuid(), "Test message contet",
             Guid.NewGuid());
         
         //act
         var exception = await Record.ExceptionAsync(async () => await Act(command));
         
         //assert
         exception.ShouldBeOfType<TicketNotFoundException>();
     }
     
     [Fact]
     public async Task HandleAsync_GivenNotExistingActiveUserDto_ShouldThrowActiveUserNotFoundException()
     {
         //arrange
         var ticket = TicketsFactory.Get();
         var command = new AddMessageCommand(Guid.NewGuid(), Guid.NewGuid(), "Test message content",
             ticket.Id);
         
         _ticketRepository
             .GetByIdAsync(command.TicketId)
             .Returns(ticket);
         
         //act
         var exception = await Record.ExceptionAsync(async() => await Act(command));
         
         //assert
         exception.ShouldBeOfType<ActiveUserNotFoundException>();
     }

    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly IMessageBroker _messageBroker;
    private readonly DateTime _now = DateTime.Now;
    private readonly AddMessageCommandHandler _handler;

    public AddMessageCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _messageBroker = Substitute.For<IMessageBroker>();
        var clock = TestsClock.Create(_now);
        _handler = new AddMessageCommandHandler(_ticketRepository, _ownerApiClient, _messageBroker,
            clock);
    }
    #endregion
}