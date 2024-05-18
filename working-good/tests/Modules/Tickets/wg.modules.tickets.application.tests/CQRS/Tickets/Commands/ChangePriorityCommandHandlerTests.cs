using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangePriority;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class ChangePriorityCommandHandlerTests
{
    private Task Act(ChangePriorityCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenNotPriorityTicketExpirationDate_ShouldUpdateTicketByRepositoryWithIsPriorityAsTrue()
    {
        //arrange 
        var ticket = TicketsFactory.Get();
        var employeeId = Guid.NewGuid();
        var companySlaTime = new SlaTimeDto()
        {
            Value = TimeSpan.FromHours(4)
        };
        ticket.ChangeAssignedEmployee(employeeId);

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        _companiesApiClient
            .GetSlaTimeByEmployeeAsync(new EmployeeIdDto(employeeId))
            .Returns(companySlaTime);
        
        var command = new ChangePriorityCommand(ticket.Id);
        
        //act
        await Act(command);
        
        //assert
        ticket.IsPriority.Value.ShouldBeTrue();
        ticket.ExpirationDate.Value.ShouldBe(_now.Add(companySlaTime.Value));

        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }
    
    
    [Fact]
    public async Task HandleAsync_GivenPriorityTicket_ShouldUpdateTicketByRepositoryWithIsPriorityAsFalse()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        ticket.ChangeAssignedEmployee(Guid.NewGuid());
        ticket.ChangePriority(true, TimeSpan.FromHours(1), DateTime.Now);

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        
        var command = new ChangePriorityCommand(ticket.Id);

        //act
        await Act(command);
        
        //assert
        ticket.IsPriority.Value.ShouldBeFalse();
        
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingTicket_ShouldThrowTicketNotFoundException()
    {
        //arrange
        var command = new ChangePriorityCommand(Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly DateTime _now;
    private readonly ICommandHandler<ChangePriorityCommand> _handler;
    
    public ChangePriorityCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _now = DateTime.Now;
        
        _handler = new ChangePriorityCommandHandler(_ticketRepository, _companiesApiClient, TestsClock.Create(_now));
    }
    #endregion
}