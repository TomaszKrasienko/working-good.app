using Microsoft.AspNetCore.Http.Features;
using NSubstitute;
using NSubstitute.Extensions;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.shared.abstractions.Events;
using wg.shared.abstractions.Time;
using wg.sharedForTests.Mocks;
using Xunit;

namespace wg.modules.tickets.application.tests;

public sealed class AddTicketCommandHandlerTests
{
    private Task Act(AddTicketCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenNotPriorityTicketExistingAssignings_ShouldAddTicketByRepositoryAndSendEvent()
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
            .IsEmployeeExists(assignedEmployee)
            .Returns(true);
        _companiesApiClient
            .IsProjectExists(projectId)
            .Returns(true);
        _ownerApiClient
            .IsUserInGroup(Arg.Is<UserInGroupDto>(arg
                => arg.UserId == assignedUser
                   && arg.GroupId == projectId))
            .Returns(true);
        
        var command = new AddTicketCommand(Guid.NewGuid(), "Test subject", "Test content",
            Guid.NewGuid(), State.New(), false, assignedEmployee, assignedUser, 
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
               && arg.State.Value == command.State
               && arg.IsPriority.Value == command.IsPriority
               && arg.AssignedEmployee.Value == command.AssignedEmployee
               && arg.AssignedUser.Value == command.AssignedUser
               && arg.ProjectId.Value == command.ProjectId));
        await _eventDispatcher
            .Received(1)
            .PublishAsync<TicketCreated>(Arg.Is<TicketCreated>(x 
                => x.Subject == command.Subject
                && x.Content == command.Content
                && x.TicketNumber == maxNumber + 1
                && x.EmployeeId == assignedEmployee
                && x.UserId == assignedUser));
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly IOwnerApiClient _ownerApiClient;
    private readonly IClock _clock;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly AddTicketCommandHandler _handler;

    public AddTicketCommandHandlerTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _ownerApiClient = Substitute.For<IOwnerApiClient>();
        _clock = TestsClock.Create();
        _eventDispatcher = Substitute.For<IEventDispatcher>();
        _handler = new AddTicketCommandHandler(_ticketRepository, _companiesApiClient, _ownerApiClient,
            _clock, _eventDispatcher);
    }
    #endregion
}