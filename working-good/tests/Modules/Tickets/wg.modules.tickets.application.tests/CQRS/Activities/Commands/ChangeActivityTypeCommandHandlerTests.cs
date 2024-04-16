using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.CQRS.Activities.Commands.ChangeActivityType;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Activities.Commands;

public sealed class ChangeActivityTypeCommandHandlerTests
{
    private Task Act(ChangeActivityTypeCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingActivityId_ShouldUpdateTicketByRepository()
    {
        //arrange
        var ticket = TicketsFactory.GetOnlyRequired(state: State.Open()).Single();
        var activity = ActivityFactory.GetInTicket(ticket).Single();
        var command = new ChangeActivityTypeCommand(activity.Id);
        _ticketRepository
            .GetByActivityId(command.Id)
            .Returns(ticket);

        //act
        await Act(command);
        
        //assert
        var updatedActivity = ticket.Activities.FirstOrDefault(x => x.Id.Equals(command.Id));
        updatedActivity?.IsPaid.Value.ShouldBe(!activity.IsPaid);
        
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }

    [Fact]
    public async Task HandleAsync_GivenNonExistingActivityId_ShouldThrowActivityNotFoundException()
    {
        //arrange
        var command = new ChangeActivityTypeCommand(Guid.NewGuid());
        await _ticketRepository
            .GetByActivityId(command.Id);

        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<ActivityNotFoundException>();
    }
    
    #region arrange

    private readonly ITicketRepository _ticketRepository;
    private readonly ICommandHandler<ChangeActivityTypeCommand> _handler;

    public ChangeActivityTypeCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _handler = new ChangeActivityTypeCommandHandler(_ticketRepository);
    }
    #endregion
}