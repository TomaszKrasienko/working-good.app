using System.Runtime.InteropServices.JavaScript;
using NSubstitute;
using Shouldly;
using wg.modules.activities.application.Clients;
using wg.modules.activities.application.Clients.DTOs;
using wg.modules.activities.application.CQRS.AddActivity;
using wg.modules.activities.application.Exceptions;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Activities;
using wg.tests.shared.Helpers;
using Xunit;

namespace wg.modules.activities.application.tests.CQRS.Commands;

public sealed class AddActivityCommandHandlerTests
{
    private Task Act(AddActivityCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingTicketWithStateForChangesAndNewDailyEmployeeActivity_ShouldAddActivityByRepository()
    {
        //arrange
        var command = new AddActivityCommand(Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid(),
            "Test activity contet",
            ShortDateTimeProvider.Get(DateTime.Now.AddHours(-1)),
            ShortDateTimeProvider.Get(DateTime.Now), true);
        
        await _dailyEmployeeActivityRepository
            .GetByDateForUser(command.TimeFrom.Date, command.UserId);
        _ticketsApiClient
            .IsAvailableForChangesTicketExists(Arg.Is<TicketIdDto>(arg => arg.TicketId == command.TicketId))
            .Returns(new TicketExistsDto()
            {
                Value = true
            });
        
        
        //act
        await Act(command);
        
        //assert
        await _dailyEmployeeActivityRepository
            .AddAsync(Arg.Is<DailyEmployeeActivity>(arg
                => arg.UserId.Equals(command.UserId)
                   && arg.Day.Value == command.TimeFrom.Date
                   && arg.Activities.Any(a
                       => a.Id.Equals(command.Id)
                          && a.TicketId.Equals(command.TicketId)
                          && a.Content == command.Content
                          && a.ActivityTime.TimeFrom == command.TimeFrom
                          && a.ActivityTime.TimeTo == command.TimeTo)));
    }

    [Fact]
    public async Task HandleAsync_GivenExistingTicketWithStateForChangesAndExistingDailyEmployeeActivity_ShouldUpdateDailyEmployeeActivity()
    {
        //arrange
        var dailyEmployeeActivity = DailyEmployeeActivityFactory.Get();
        var ticketId = Guid.NewGuid();
        
        dailyEmployeeActivity.AddPaidActivity(Guid.NewGuid(), "Test content",
            Guid.NewGuid(), new DateTime(dailyEmployeeActivity.Day.Value.Year, 
                dailyEmployeeActivity.Day.Value.Month, dailyEmployeeActivity.Day.Value.Day, 8,0,0),
            new DateTime(dailyEmployeeActivity.Day.Value.Year, 
                dailyEmployeeActivity.Day.Value.Month, dailyEmployeeActivity.Day.Value.Day, 9,0,0));
        
        var command = new AddActivityCommand(Guid.NewGuid(), dailyEmployeeActivity.UserId,ticketId,
            "Test activity content",
            new DateTime(dailyEmployeeActivity.Day.Value.Year, dailyEmployeeActivity.Day.Value.Month, 
                dailyEmployeeActivity.Day.Value.Day, 9,0,0),
            new DateTime(dailyEmployeeActivity.Day.Value.Year, dailyEmployeeActivity.Day.Value.Month, 
                dailyEmployeeActivity.Day.Value.Day, 10,0,0),
            true);
        
        
        _dailyEmployeeActivityRepository
            .GetByDateForUser(command.TimeFrom.Date, command.UserId)
            .Returns(dailyEmployeeActivity);
        
        _ticketsApiClient
            .IsAvailableForChangesTicketExists(Arg.Is<TicketIdDto>(arg => arg.TicketId == command.TicketId))
            .Returns(new TicketExistsDto()
            {
                Value = true
            });
        
        //act
        await Act(command);
        
        //assert
        await _dailyEmployeeActivityRepository
            .Received(1)
            .UpdateAsync(dailyEmployeeActivity);
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingTicketWithStateForChanges_ShouldThrowTicketWithStateForChangesNotFound()
    {
        //arrange
        var command = new AddActivityCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            "Test content", DateTime.Now.AddHours(-1), DateTime.Now, false);
        
        _ticketsApiClient
            .IsAvailableForChangesTicketExists(Arg.Is<TicketIdDto>(arg => arg.TicketId == command.TicketId))
            .Returns(new TicketExistsDto()
            {
                Value = false
            });
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<TicketWithStateForChangesNotFoundException>();
    }
    
    #region arrange

    private readonly IDailyEmployeeActivityRepository _dailyEmployeeActivityRepository;
    private readonly ITicketsApiClient _ticketsApiClient;
    private readonly ICommandHandler<AddActivityCommand> _handler;

    public AddActivityCommandHandlerTests()
    {
        _dailyEmployeeActivityRepository = Substitute.For<IDailyEmployeeActivityRepository>();
        _ticketsApiClient = Substitute.For<ITicketsApiClient>();
        _handler = new AddActivityCommandHandler(_dailyEmployeeActivityRepository,
            _ticketsApiClient);
    }
    #endregion
}