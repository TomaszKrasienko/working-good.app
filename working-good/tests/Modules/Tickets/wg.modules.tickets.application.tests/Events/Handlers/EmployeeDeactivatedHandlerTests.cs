using NSubstitute;
using wg.modules.tickets.application.Events.External;
using wg.modules.tickets.application.Events.External.Handlers;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Time;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.tickets.application.tests.Events.Handlers;

public sealed class EmployeeDeactivatedHandlerTests
{
    private Task Act(EmployeeDeactivated @event) => _handler.HandleAsync(@event);
    
    [Fact]
    public async Task HandleAsync_GivenEmployeeDeactivatedEvent_ShouldUpdateAllTickets()
    {
        //arrange
        var @event = new EmployeeDeactivated(Guid.NewGuid(), Guid.NewGuid());
        var tickets = TicketsFactory.GetOnlyRequired(4);
        foreach (var ticket in tickets)
        {
            ticket.ChangeAssignedEmployee(@event.EmployeeId);
        }

        _ticketRepository
            .GetAllForAssignedEmployee(@event.EmployeeId)
            .Returns(tickets);
        
        //act
        await Act(@event);
        
        //assert
        await _ticketRepository
            .Received(tickets.Count)
            .UpdateAsync(Arg.Is<Ticket>(x => tickets.Contains(x)));
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly IClock _clock;
    private readonly EmployeeDeactivatedHandler _handler;

    public EmployeeDeactivatedHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _clock = TestsClock.Create();
        _handler = new EmployeeDeactivatedHandler(_ticketRepository, _clock);
    }
    #endregion
}