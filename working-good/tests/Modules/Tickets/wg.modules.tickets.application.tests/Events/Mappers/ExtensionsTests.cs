using Shouldly;
using wg.modules.tickets.application.Events.Mappers;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.sharedForTests.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.Events.Mappers;

public class ExtensionsTests
{
    [Fact]
    public void AsEvent_GivenTicket_ShouldReturnTicketCreated()
    {
        //arrange
        var ticket = TicketsFactory.GetAll(State.New());
        
        //act
        var result = ticket.AsEvent();
        
        //assert
        result.Id.ShouldBe(ticket.Id.Value);
        result.Subject.ShouldBe(ticket.Subject.Value);
        result.Content.ShouldBe(ticket.Content.Value);
        result.TicketNumber.ShouldBe(ticket.Number.Value);
        result.EmployeeId.ShouldBe(ticket.AssignedEmployee.Value);
        result.UserId.ShouldBe(ticket.AssignedUser.Value);
    } 
}