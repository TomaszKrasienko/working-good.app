using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignProject;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Tickets;
using Xunit;
using Xunit.Sdk;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class AssignProjectCommandHandlerTests
{
    private Task Act(AssignProjectCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenTicketWithUserAndValidMembership_ShouldAssignProjectUpdateTicket()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var userId = Guid.NewGuid();
        ticket.ChangeAssignedUser(userId);
        var command = new AssignProjectCommand(ticket.Id, Guid.NewGuid());

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        _ownerApiClient
            .IsMembershipExistsAsync(Arg.Is<GetMembershipDto>(arg
                => arg.UserId == userId
                   && arg.GroupId == command.ProjectId))
            .Returns(new IsGroupMembershipExists()
            {
                Value = true
            });
        
        //act
        await Act(command);
        
        //assert
        ticket.ProjectId.Value.ShouldBe(command.ProjectId);

        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }

    [Fact]
    public async Task HandleAsync_GivenTicketWithEmployeeAndProjectForCompany_ShouldAssignProjectUpdateTicket()
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
                => arg.EmployeeId == employeeId 
                   && arg.ProjectId == command.ProjectId))
            .Returns(new IsProjectForCompanyDto()
            {
                Value = true
            });
        
        //act
        await Act(command);
        
        //assert
        ticket.ProjectId.Value.ShouldBe(command.ProjectId);

        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }

    [Fact]
    public async Task HandleAsync_GivenActiveProject_ShouldAssignProjectUpdateTicket()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var command = new AssignProjectCommand(ticket.Id, Guid.NewGuid());

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        _companiesApiClient
            .IsProjectActiveAsync(Arg.Is<ProjectIdDto>(arg => arg.Id == command.ProjectId))
            .Returns(new IsProjectActiveDto()
            {
                Value = true
            });
        
        //act
        await Act(command);
        
        //assert
        ticket.ProjectId.Value.ShouldBe(command.ProjectId);

        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }

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
    public async Task
        HandleAsync_GivenTicketWithEmployeeAndProjectNotFromEmployeeCompany_ShouldThrowInvalidProjectForEmployeeException()
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

    [Fact]
    public async Task HandleAsync_GivenTicketWithUserAndValidMembership_ShouldThrowUserDoesNotBelongToGroupException()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var userId = Guid.NewGuid();
        ticket.ChangeAssignedUser(userId);
        var command = new AssignProjectCommand(ticket.Id, Guid.NewGuid());

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        _ownerApiClient
            .IsMembershipExistsAsync(Arg.Is<GetMembershipDto>(arg
                => arg.GroupId == command.ProjectId
                   && arg.UserId == userId))
            .Returns(new IsGroupMembershipExists()
            {
                Value = false
            });

        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));

        //assert
        exception.ShouldBeOfType<UserDoesNotBelongToGroupException>();
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingActiveProject_ShouldThrowActiveProjectNotFoundException()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var command = new AssignProjectCommand(ticket.Id, Guid.NewGuid());

        _ticketRepository
            .GetByIdAsync(ticket.Id)
            .Returns(ticket);

        _companiesApiClient
            .IsProjectActiveAsync(Arg.Is<ProjectIdDto>(arg => arg.Id == command.ProjectId))
            .Returns(new IsProjectActiveDto()
            {
                Value = false
            });
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<ActiveProjectNotFoundException>();
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
        _handler = new AssignProjectCommandHandler(_ticketRepository, _ownerApiClient,
            _companiesApiClient);
    }

    #endregion
}