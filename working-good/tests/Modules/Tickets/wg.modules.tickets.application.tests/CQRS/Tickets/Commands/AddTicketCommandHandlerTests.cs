using NSubstitute;
using Shouldly;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Events;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.tickets.application.tests.CQRS.Tickets.Commands;

public sealed class AddTicketCommandHandlerTests
{
    private Task Act(AddTicketCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenNotPriorityTicketExistingAssignedIds_ShouldAddTicketByRepositoryAndSendEvent()
    {
        //arrange
        var assignedUser = Guid.NewGuid();
        var maxNumber = 1;

        var employeeDto = new EmployeeDto()
        {
            Id = Guid.NewGuid(),
            Email = "test@test.pl",
            IsActive = true,
            PhoneNumber = "515515515"
        };

        var projectDto = new ProjectDto()
        {
            Id = Guid.NewGuid(),
            Description = "MyTestProject",
            PlannedStart = DateTime.Now.AddMonths(-1),
            PlannedFinish = DateTime.Now.AddMonths(5),
            Title = "Test"
        };

        var companyDto = new CompanyDto()
        {
            SlaTime = TimeSpan.FromHours(8),
            Employees = [employeeDto],
            Projects = [projectDto]
        };

        _companiesApiClient
            .GetCompanyByEmployeeIdAsync(Arg.Is<EmployeeIdDto>(arg => arg.EmployeeId == employeeDto.Id))
            .Returns(companyDto);

        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(maxNumber);

        _ownerApiClient
            .IsUserExistsAsync(Arg.Is<UserIdDto>(arg => arg.Id == assignedUser))
            .Returns(new IsUserExistsDto(){Value = true});
        _ownerApiClient
            .IsUserInGroupAsync(Arg.Is<UserInGroupDto>(arg
                => arg.UserId == assignedUser
                   && arg.GroupId == projectDto.Id))
            .Returns(new IsUserInGroupDto(){Value = true});
        
        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content",
            Guid.NewGuid(), State.New(), false, employeeDto.Id, assignedUser, 
            projectDto.Id);
        
        //act
        await Act(command);
        
        //assert
        await _ticketRepository
            .Received(1)
            .AddAsync(Arg.Is<Ticket>(arg
                => arg.Id.Value == command.Id
               && arg.Subject.Value == command.Subject
               && arg.Content.Value == command.Content
               && arg.CreatedBy.Value == command.CreatedBy
               && arg.State.Value == State.Open()
               && arg.IsPriority.Value == command.IsPriority
               && arg.AssignedEmployee.Value == command.AssignedEmployee
               && arg.AssignedUser.Value == command.AssignedUser
               && arg.ProjectId.Value == command.ProjectId));
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<TicketCreated>(x 
                => x.Subject == command.Subject
                && x.Content == command.Content
                && x.TicketNumber == maxNumber + 1
                && x.EmployeeId == employeeDto.Id
                && x.UserId == assignedUser));
    }
    
   [Fact]
    public async Task HandleAsync_GivenNotPriorityTicketWithoutProject_ShouldAddTicketByRepositoryAndSendEvent()
    {
        //arrange
        var assignedEmployee = Guid.NewGuid();
        var assignedUser = Guid.NewGuid();
        var maxNumber = 1;

        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(maxNumber);
        _companiesApiClient
            .IsEmployeeExistsAsync(Arg.Is<EmployeeIdDto>(arg => arg.EmployeeId == assignedEmployee))
            .Returns(new IsEmployeeExistsDto(){ Value = true});
        _ownerApiClient
            .IsUserExistsAsync(Arg.Is<UserIdDto>(arg => arg.Id == assignedUser))
            .Returns(new IsUserExistsDto(){Value = true});
        
        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content",
            Guid.NewGuid(), State.New(), false, assignedEmployee, assignedUser, 
            null);
        
        //act
        await Act(command);
        
        //assert
        await _ticketRepository
            .Received(1)
            .AddAsync(Arg.Is<Ticket>(arg
                => arg.Id.Value == command.Id
               && arg.Subject.Value == command.Subject
               && arg.Content.Value == command.Content
               && arg.CreatedBy.Value == command.CreatedBy
               && arg.State.Value == State.Open()
               && arg.IsPriority.Value == command.IsPriority
               && arg.AssignedEmployee.Value == command.AssignedEmployee
               && arg.AssignedUser.Value == command.AssignedUser
               && arg.ProjectId == null));
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<TicketCreated>(x 
                => x.Subject == command.Subject
                && x.Content == command.Content
                && x.TicketNumber == maxNumber + 1
                && x.EmployeeId == assignedEmployee
                && x.UserId == assignedUser));
    }
    
    [Fact]
    public async Task HandleAsync_GivenPriorityTicketExistingAssignedIds_ShouldAddTicketByRepositoryAndSendEvent()
    {
        //arrange
        var assignedEmployee = Guid.NewGuid();
        var assignedUser = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var maxNumber = 1;
        var companyDto = new CompanyDto()
        {
            SlaTime = TimeSpan.FromHours(1)
        };
        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(maxNumber);
        _companiesApiClient
            .IsEmployeeExistsAsync(Arg.Is<EmployeeIdDto>(arg => arg.EmployeeId == assignedEmployee))
            .Returns(new IsEmployeeExistsDto(){ Value = true});
        _companiesApiClient
            .IsProjectExistsAsync(Arg.Is<EmployeeWithProjectDto>(arg 
                => arg.ProjectId == projectId && arg.EmployeeId == assignedEmployee))
            .Returns(new IsProjectExistsDto(){ Value = true});
        _companiesApiClient
            .GetCompanyByEmployeeIdAsync(Arg.Is<EmployeeIdDto>(arg => arg.EmployeeId == assignedEmployee))
            .Returns(companyDto);
        _ownerApiClient
            .IsUserExistsAsync(Arg.Is<UserIdDto>(arg => arg.Id == assignedUser))
            .Returns(new IsUserExistsDto(){Value = true});
        _ownerApiClient
            .IsUserInGroupAsync(Arg.Is<UserInGroupDto>(arg
                => arg.UserId == assignedUser
                   && arg.GroupId == projectId))
            .Returns(new IsUserInGroupDto(){Value = true});
        
        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content",
            Guid.NewGuid(), State.New(), true, assignedEmployee, assignedUser, 
            projectId);
        
        //act
        await Act(command);
        
        //assert
        await _ticketRepository
            .Received(1)
            .AddAsync(Arg.Is<Ticket>(arg
                => arg.Id.Value == command.Id
               && arg.Subject.Value == command.Subject
               && arg.Content.Value == command.Content
               && arg.CreatedBy.Value == command.CreatedBy
               && arg.State.Value == State.Open()
               && arg.IsPriority.Value == command.IsPriority
               && arg.AssignedEmployee.Value == command.AssignedEmployee
               && arg.AssignedUser.Value == command.AssignedUser
               && arg.ProjectId.Value == command.ProjectId
               && arg.ExpirationDate.Value == _clock.Now().Add(companyDto.SlaTime)));
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<TicketCreated>(x 
                => x.Subject == command.Subject
                && x.Content == command.Content
                && x.TicketNumber == maxNumber + 1
                && x.EmployeeId == assignedEmployee
                && x.UserId == assignedUser));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotPriorityTicketWithoutAssignedIds_ShouldAddTicketByRepositoryAndSendEvent()
    {
        //arrange
        var maxNumber = 1;
        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(maxNumber);
        await _companiesApiClient
            .Received(0)
            .IsEmployeeExistsAsync(Arg.Any<EmployeeIdDto>());
        await _companiesApiClient
            .Received(0)
            .IsProjectExistsAsync(Arg.Any<EmployeeWithProjectDto>());
        await _companiesApiClient
            .Received(0)
            .GetCompanyByEmployeeIdAsync(Arg.Any<EmployeeIdDto>());
        await _ownerApiClient
            .Received(0)
            .IsUserExistsAsync(Arg.Any<UserIdDto>());
        await _ownerApiClient
            .Received(0)
            .IsUserInGroupAsync(Arg.Any<UserInGroupDto>());
        
        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content",
            Guid.NewGuid(), State.New(), false, null, null, null);
        
        //act
        await Act(command);
        
        //assert
        await _ticketRepository
            .Received(1)
            .AddAsync(Arg.Is<Ticket>(arg
                => arg.Id.Value == command.Id
                   && arg.Subject.Value == command.Subject
                   && arg.Content.Value == command.Content
                   && arg.CreatedBy.Value == command.CreatedBy
                   && arg.State.Value == command.State
                   && arg.IsPriority.Value == command.IsPriority
                   && arg.AssignedEmployee == null
                   && arg.AssignedUser == null
                   && arg.ProjectId == null
                   && arg.ExpirationDate == null));
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<TicketCreated>(x 
                => x.Subject == command.Subject
                   && x.Content == command.Content
                   && x.TicketNumber == maxNumber + 1
                   && x.EmployeeId == null
                   && x.UserId == null));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingEmployee_ShouldThrowEmployeeDoesNotExistException()
    {
        //arrange
        var assignedEmployee = Guid.NewGuid();
        var assignedUser = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var maxNumber = 1;
        var slaTimeDto = new CompanySlaTimeDto()
        {
            SlaTime = TimeSpan.FromHours(1)
        };
        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(maxNumber);
        _companiesApiClient
            .IsEmployeeExistsAsync(Arg.Is<EmployeeIdDto>(arg => arg.EmployeeId == assignedEmployee))
            .Returns(new IsEmployeeExistsDto(){ Value = false});
        
        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content",
            Guid.NewGuid(), State.New(), true, assignedEmployee, assignedUser, 
            projectId);
        
        //act
        var exception = await Record.ExceptionAsync( async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<EmployeeDoesNotExistException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingProject_ShouldThrowProjectDoesNotExists()
    {
        //arrange
        var assignedEmployee = Guid.NewGuid();
        var assignedUser = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var maxNumber = 1;
        var slaTimeDto = new CompanySlaTimeDto()
        {
            SlaTime = TimeSpan.FromHours(1)
        };
        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(maxNumber);
        _companiesApiClient
            .IsEmployeeExistsAsync(Arg.Is<EmployeeIdDto>(arg => arg.EmployeeId == assignedEmployee))
            .Returns(new IsEmployeeExistsDto(){ Value = true});
        _companiesApiClient
            .IsProjectExistsAsync(Arg.Is<EmployeeWithProjectDto>(arg 
                => arg.ProjectId == projectId && arg.EmployeeId == assignedEmployee))
            .Returns(new IsProjectExistsDto(){ Value = false});
        
        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content",
            Guid.NewGuid(), State.New(), true, assignedEmployee, assignedUser, 
            projectId);
        
        //act
        var exception = await Record.ExceptionAsync(async () =>  await Act(command));
        
        //assert
        exception.ShouldBeOfType<ProjectDoesNotExists>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenUserNotInGroup_ShouldThrowUserDoesNotBelongToGroupException()
    {
        //arrange
        var assignedEmployee = Guid.NewGuid();
        var assignedUser = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var maxNumber = 1;

        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(maxNumber);
        _companiesApiClient
            .IsEmployeeExistsAsync(Arg.Is<EmployeeIdDto>(arg => arg.EmployeeId == assignedEmployee))
            .Returns(new IsEmployeeExistsDto(){ Value = true});
        _companiesApiClient
            .IsProjectExistsAsync(Arg.Is<EmployeeWithProjectDto>(arg 
                => arg.ProjectId == projectId && arg.EmployeeId == assignedEmployee))
            .Returns(new IsProjectExistsDto(){ Value = true});
        _ownerApiClient
            .IsUserExistsAsync(Arg.Is<UserIdDto>(arg => arg.Id == assignedUser))
            .Returns(new IsUserExistsDto(){Value = true});
        _ownerApiClient
            .IsUserInGroupAsync(Arg.Is<UserInGroupDto>(arg
                => arg.UserId == assignedUser
                   && arg.GroupId == projectId))
            .Returns(new IsUserInGroupDto(){Value = false});
        
        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content",
            Guid.NewGuid(), State.New(), false, assignedEmployee, assignedUser, 
            projectId);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<UserDoesNotBelongToGroupException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingUser_ShouldThrowUserDoesNotExistException()
    {
        //arrange
        var assignedEmployee = Guid.NewGuid();
        var assignedUser = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var maxNumber = 1;

        _ticketRepository
            .GetMaxNumberAsync()
            .Returns(maxNumber);
        _companiesApiClient
            .IsEmployeeExistsAsync(Arg.Is<EmployeeIdDto>(arg => arg.EmployeeId == assignedEmployee))
            .Returns(new IsEmployeeExistsDto(){ Value = true});
        _companiesApiClient
            .IsProjectExistsAsync(Arg.Is<EmployeeWithProjectDto>(arg 
                => arg.ProjectId == projectId && arg.EmployeeId == assignedEmployee))
            .Returns(new IsProjectExistsDto(){ Value = true});
        _ownerApiClient
            .IsUserExistsAsync(Arg.Is<UserIdDto>(arg => arg.Id == assignedUser))
            .Returns(new IsUserExistsDto(){Value = false});
        
        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content",
            Guid.NewGuid(), State.New(), false, assignedEmployee, assignedUser, 
            projectId);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<UserDoesNotExistException>();
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;
    private readonly AddTicketCommandHandler _handler;

    public AddTicketCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _clock = TestsClock.Create(DateTime.Now);
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new AddTicketCommandHandler(_ticketRepository, _companiesApiClient, _ownerApiClient,
            _clock, _messageBroker);
    }
    #endregion
}