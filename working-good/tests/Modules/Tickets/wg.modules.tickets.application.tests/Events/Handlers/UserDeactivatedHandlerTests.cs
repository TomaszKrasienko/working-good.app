// using NSubstitute;
// using Shouldly;
// using wg.modules.tickets.application.Events.External;
// using wg.modules.tickets.application.Events.External.Handlers;
// using wg.modules.tickets.domain.Repositories;
// using wg.modules.tickets.domain.ValueObjects.Ticket;
// using wg.shared.abstractions.Events;
// using wg.tests.shared.Factories.Tickets;
// using Xunit;
//
// namespace wg.modules.tickets.application.tests.Events.Handlers;
//
// public sealed class UserDeactivatedHandlerTests
// {
//     private Task Act(UserDeactivated @event) => _handler.HandleAsync(@event);
//     
//     [Fact]
//     public async Task HandleAsync_GivenExistingTicketsForUserId_ShouldUpdateAsAssignedUserToNull()
//     {
//         //arrange
//         var @event = new UserDeactivated(Guid.NewGuid());
//         var tickets = TicketsFactory.Get(2);
//         tickets[0].ChangeAssignedUser(@event.UserId, DateTime.Now);
//         tickets[1].ChangeAssignedUser(@event.UserId, DateTime.Now);
//
//         _ticketRepository
//             .GetAllForAssignedUser(@event.UserId)
//             .Returns(tickets);
//         
//         //act
//         await Act(@event);
//         
//         //assert
//         await _ticketRepository
//             .Received(1)
//             .UpdateAsync(tickets[0]);
//         
//         await _ticketRepository
//             .Received(1)
//             .UpdateAsync(tickets[1]);
//         
//         tickets[0].AssignedUser.ShouldBeNull();
//         tickets[1].AssignedUser.ShouldBeNull();
//     }
//     
//     [Fact]
//     public async Task HandleAsync_GivenTicketWithStateForNoChanges_ShouldNotUpdateAssignedUser()
//     {
//         //arrange
//         var @event = new UserDeactivated(Guid.NewGuid());
//         var ticket = TicketsFactory.GetOnlyRequired(State.Open());
//         ticket.ChangeAssignedUser(@event.UserId, DateTime.Now);
//         ticket.ChangeState(State.Cancelled(), DateTime.Now);
//
//         _ticketRepository
//             .GetAllForAssignedUser(@event.UserId)
//             .Returns([ticket]);
//         
//         //act
//         await Act(@event);
//         
//         //assert
//         await _ticketRepository
//             .Received(1)
//             .UpdateAsync(ticket);
//         
//         ticket.AssignedUser.Value.ShouldBe(@event.UserId);
//     }
//     
//     #region arrange
//
//     private readonly ITicketRepository _ticketRepository;
//     private readonly IEventHandler<UserDeactivated> _handler;
//
//     public UserDeactivatedHandlerTests()
//     {
//         _ticketRepository = Substitute.For<ITicketRepository>();
//         _handler = new UserDeactivatedHandler(_ticketRepository);
//     }
//     #endregion
// }