using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignEmployee;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class AssignEmployeeCommandHandlerTests
{
    private Task Act(AssignEmployeeCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingActiveEmployee_ShouldAssignEmployeeAndUpdateTicketAndSendEvent()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var command = new AssignEmployeeCommand(Guid.NewGuid(), ticket.Id);

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        
        _companiesApiClient
            .IsActiveEmployeeExistsAsync(new EmployeeIdDto(command.EmployeeId))
            .Returns(new IsActiveEmployeeExistsDto()
            {
                Value = true
            });

        //act
        await Act(command);

        //assert
        ticket.AssignedEmployee.Value.ShouldBe(command.EmployeeId);

        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);

        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<EmployeeAssigned>(arg
                => arg.TicketId.Equals(ticket.Id)
                   && arg.TicketNumber == ticket.Number
                   && arg.EmployeeId.Equals(command.EmployeeId)));
    }
    
    [Fact]
    public async Task HandleAsync_GivenTicketWithProjectAndEmployeeForCompany_ShouldAssignEmployeeUpdateTicketAndSentEvent()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var projectId = Guid.NewGuid();
        ticket.ChangeProject(projectId);
        var command = new AssignEmployeeCommand(Guid.NewGuid(), ticket.Id);

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        _companiesApiClient
            .IsProjectForCompanyAsync(Arg.Is<EmployeeWithProjectDto>(arg
                => arg.EmployeeId == command.EmployeeId
                   && arg.ProjectId == projectId))
            .Returns(new IsProjectForCompanyDto()
            {
                Value = true
            });
        
        //act
        await Act(command);
        
        //arrange
        ticket.ProjectId.Value.ShouldBe(projectId);

        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
        
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<EmployeeAssigned>(arg
                => arg.TicketId.Equals(ticket.Id)
                   && arg.TicketNumber == ticket.Number
                   && arg.EmployeeId.Equals(command.EmployeeId)));
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingTicketId_ShouldReturnTicketNotFoundException()
    {
        //arrange
        var command = new AssignEmployeeCommand(Guid.NewGuid(), Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_GivenTicketForProjectAndEmployeeIsNotFromCompanyWithProject_ShouldThrowInvalidProjectForEmployeeException()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var projectId = Guid.NewGuid();
        ticket.ChangeProject(projectId);

        var command = new AssignEmployeeCommand(Guid.NewGuid(), ticket.Id);
        
        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        _companiesApiClient
            .IsProjectForCompanyAsync(Arg.Is<EmployeeWithProjectDto>(arg 
                => arg.EmployeeId.Equals(command.EmployeeId)
                && arg.ProjectId == projectId))
            .Returns(new IsProjectForCompanyDto()
            {
                Value = false
            });
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<InvalidProjectForEmployeeException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActiveEmployee_ShouldThrowActiveEmployeeNotFoundException()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var command = new AssignEmployeeCommand(Guid.NewGuid(), ticket.Id);
        
        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        _companiesApiClient
            .IsActiveEmployeeExistsAsync(new EmployeeIdDto(command.EmployeeId))
            .Returns(new IsActiveEmployeeExistsDto()
            {
                Value = false
            });
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<ActiveEmployeeNotFoundException>();
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly IMessageBroker _messageBroker;
    private readonly ICommandHandler<AssignEmployeeCommand> _handler;

    public AssignEmployeeCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new AssignEmployeeCommandHandler(_ticketRepository, _companiesApiClient,
            _messageBroker);
    }
    #endregion
}