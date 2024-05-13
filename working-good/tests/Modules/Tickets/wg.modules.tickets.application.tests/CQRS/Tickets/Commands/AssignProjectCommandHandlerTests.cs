using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignProject;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class AssignProjectCommandHandlerTests
{
    private Task Act(AssignProjectCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenNotExistingTicketId_ShouldReturnTicketNotFoundException()
    {
        //arrange
        var command = new AssignProjectCommand(Guid.NewGuid(), Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_GivenTicketWithEmployeeAndProjectNotFromEmployeeCompany_ShouldThrowInvalidProjectForEmployeeException()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var employeeId = Guid.NewGuid();
        ticket.ChangeAssignedEmployee(employeeId);
        var command = new AssignProjectCommand(ticket.Id, Guid.NewGuid());
        
        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);
        
        _companiesApiClient
            .IsProjectForCompanyAsync(Arg.Is<EmployeeWithProjectDto>(arg 
                => arg.EmployeeId.Equals(employeeId)
                   && arg.ProjectId == command.ProjectId))
            .Returns(new IsProjectForCompanyDto()
            {
                Value = false
            });
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<InvalidProjectForEmployeeException>();
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly ICommandHandler<AssignProjectCommand> _handler;
    
    public AssignProjectCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _handler = new AssignProjectCommandHandler(_ticketRepository, _ownerApiClient);
    }

    #endregion
}