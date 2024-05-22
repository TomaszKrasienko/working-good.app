using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketExpirationDate;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class ChangeTicketExpirationDateCommandHandlerTests
{
    private Task Act(ChangeTicketExpirationDateCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingTicketWithAssignedEmployee_ShouldChangeTicketExpirationDateAndUpdate()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var employeeId = Guid.NewGuid();
        ticket.ChangeAssignedEmployee(employeeId);

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        _companiesApiClient
            .GetSlaTimeByEmployeeAsync(Arg.Is<EmployeeIdDto>(arg
                => arg.EmployeeId == employeeId))
            .Returns(new SlaTimeDto()
            {
                Value = TimeSpan.FromDays(3)
            });

        var command = new ChangeTicketExpirationDateCommand(ticket.Id, DateTime.Now.AddHours(4));
        
        //act
        await Act(command);
        
        //assert
        ticket.ExpirationDate.Value.ShouldBe(command.ExpirationDate);

        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }

    [Fact]
    public async Task HandleAsync_GivenExistingTicketWithoutAssignedEmployee_ShouldChangeTicketExpirationDateAndUpdate()
    {
        //arrange
        var ticket = TicketsFactory.Get();

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        var command = new ChangeTicketExpirationDateCommand(ticket.Id, DateTime.Now.AddDays(1));

        //act
        await Act(command);
        
        //assert
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);

        await _companiesApiClient
            .Received(0)
            .GetSlaTimeByEmployeeAsync(Arg.Any<EmployeeIdDto>());
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingTicket_ShouldThrowTicketNotFoundException()
    {
        //arrange
        var command = new ChangeTicketExpirationDateCommand(Guid.NewGuid(), DateTime.Now.AddHours(8));

        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));

        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }

    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly ICommandHandler<ChangeTicketExpirationDateCommand> _handler;
    
    public ChangeTicketExpirationDateCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _handler = new ChangeTicketExpirationDateCommandHandler(_ticketRepository, _companiesApiClient);
    }
    #endregion
}