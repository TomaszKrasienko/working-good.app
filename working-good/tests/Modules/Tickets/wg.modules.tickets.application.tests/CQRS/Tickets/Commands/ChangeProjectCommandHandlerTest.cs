using System.ComponentModel.DataAnnotations.Schema;
using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeProject;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Tickets;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class ChangeProjectCommandHandlerTests
{
    private Task Act(ChangeProjectCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingTicketAssignedEmployeeAndUser_ShouldChangeProjectAndUpdateTicket()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var projectId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        ticket.ChangeAssignedEmployee(employeeId);
        ticket.ChangeAssignedUser(userId);
        
        var command = new ChangeProjectCommand(ticket.Id, projectId);
        _ticketRepository
            .GetByIdAsync(command.TicketId)
            .Returns(ticket);

        _companiesApiClient
            .IsProjectForCompanyAsync(Arg.Is<EmployeeWithProjectDto>(arg
                => arg.EmployeeId == employeeId
                   && arg.ProjectId == projectId))
            .Returns(new IsProjectForCompanyDto()
            {
                Value = true
            });
        
        _ownerApiClient
            .IsMembershipExistsAsync(Arg.Is<GetMembershipDto>(arg
                => arg.GroupId == projectId
                   && arg.UserId == userId))
            .Returns(new IsGroupMembershipExists()
            {
                Value = true
            });
        
        //act
        await Act(command);
        
        //assert
        ticket.ProjectId.Value.ShouldBe(projectId);

        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
    }

    [Fact]
    public async Task HandleAsync_GivenExistingTicketAndNotAssignedEmployeeAndUser_ShouldChangeProjectAndUpdateTicket()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var projectId = Guid.NewGuid();
        var command = new ChangeProjectCommand(ticket.Id, projectId);
        _ticketRepository
            .GetByIdAsync(command.TicketId)
            .Returns(ticket);

        _companiesApiClient
            .IsProjectActiveAsync(Arg.Is<ProjectIdDto>(arg => arg.Id.Equals(command.ProjectId)))
            .Returns(new IsProjectActiveDto()
            {
                Value = true
            });
        
        //act
        await Act(command);
        
        //assert 
        await _ticketRepository
            .Received(1)
            .UpdateAsync(ticket);
        
        ticket.ProjectId.Value.ShouldBe(command.ProjectId);
    }

        [Fact]
    public async Task HandleAsync_GivenExistingTicketAndNotExistingUserMembership_ShouldThrowUserDoesNotBelongToGroupException()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        ticket.ChangeAssignedUser(userId);
        var command = new ChangeProjectCommand(ticket.Id, projectId);
        
        _ticketRepository
            .GetByIdAsync(command.TicketId)
            .Returns(ticket);

        _ownerApiClient
            .IsMembershipExistsAsync(Arg.Is<GetMembershipDto>(arg
                => arg.GroupId == projectId
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
    public async Task HandleAsync_GivenExistingTicketAndEmployeeNotInCompanyWithProject_ShouldThrow()
    {
        //arrange
        var ticket = TicketsFactory.Get();
        var employeeId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        ticket.ChangeAssignedEmployee(employeeId);
        var command = new ChangeProjectCommand(ticket.Id, projectId);
        
        _ticketRepository
            .GetByIdAsync(command.TicketId)
            .Returns(ticket);

        _companiesApiClient
            .IsProjectForCompanyAsync(Arg.Is<EmployeeWithProjectDto>(arg
                => arg.EmployeeId == employeeId
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
    public async Task HandleAsync_GivenNotExistingTicket_ShouldThrowTicketNotFoundException()
    {
        //arrange
        var command = new ChangeProjectCommand(Guid.NewGuid(), Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketNotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingActiveProject_ShouldThrowActiveProjectNotFoundException()
    {
        //arrange 
        var ticket = TicketsFactory.Get();
        var projectId = Guid.NewGuid();
        var command = new ChangeProjectCommand(ticket.Id, projectId);
        _ticketRepository
            .GetByIdAsync(command.TicketId)
            .Returns(ticket);

        _companiesApiClient
            .IsProjectActiveAsync(Arg.Is<ProjectIdDto>(arg => arg.Id.Equals(command.ProjectId)))
            .Returns(new IsProjectActiveDto()
            {
                Value = false
            });
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<ActiveProjectNotFoundException>();
    } 
    
    #region arrage
    private readonly ITicketRepository _ticketRepository;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly ICommandHandler<ChangeProjectCommand> _handler;

    public ChangeProjectCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _handler = new ChangeProjectCommandHandler(_ticketRepository, _companiesApiClient,
            _ownerApiClient);
    }
    #endregion
}