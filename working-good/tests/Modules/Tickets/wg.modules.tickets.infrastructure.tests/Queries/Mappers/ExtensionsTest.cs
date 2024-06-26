// using Shouldly;
// using wg.modules.tickets.application.DTOs;
// using wg.modules.tickets.domain.ValueObjects.Ticket;
// using wg.tests.shared.Factories.Tickets;
// using wg.modules.tickets.infrastructure.Queries.Mappers;
// using Xunit;
//
// namespace wg.modules.tickets.infrastructure.tests.Queries.Mappers;
//
// public class ExtensionsTest
// {
//     [Fact]
//     public void AsDto_GivenMessage_ShouldReturnMessageDto()
//     {
//         //arrange
//         var message = MessagesFactory.Get();
//         
//         //act
//         var result = message.AsDto();
//         
//         //assert
//         result.ShouldBeOfType<MessageDto>();
//         result.Id.ShouldBe(message.Id.Value);
//         result.Sender.ShouldBe(message.Sender.Value);
//         result.Subject.ShouldBe(message.Subject.Value);
//         result.Content.ShouldBe(message.Content.Value);
//         result.CreatedAt.ShouldBe(message.CreatedAt.Value);
//     }
//
//     [Fact]
//     public void AsDto_GivenTicket_ShouldReturnTicketDto()
//     {
//         //arrange
//         var ticket = TicketsFactory.GetAll(State.Open());
//         
//         //act
//         var result = ticket.AsDto();
//         
//         //assert
//         result.ShouldBeOfType<TicketDto>();
//         result.Id.ShouldBe(ticket.Id.Value);
//         result.Number.ShouldBe(ticket.Number.Value);
//         result.Subject.ShouldBe(ticket.Subject.Value);
//         result.Content.ShouldBe(ticket.Content.Value);
//         result.CreatedAt.ShouldBe(ticket.CreatedAt.Value);
//         result.CreatedBy.ShouldBe(ticket.CreatedBy.Value);
//         result.IsPriority.ShouldBe(ticket.IsPriority.Value);
//         result.State.ShouldBe(ticket.State.Value);
//         result.StateChangeDate.ShouldBe(ticket.State.ChangeDate);
//         result.ExpirationDate.ShouldBe(ticket.ExpirationDate.Value);
//         result.AssignedEmployee.ShouldBe(ticket.AssignedEmployee.Value);
//         result.AssignedUser.ShouldBe(ticket.AssignedUser.Value);
//         result.ProjectId.ShouldBe(ticket.ProjectId.Value);
//         result.Messages.ShouldBeEmpty();
//     }
//
// }