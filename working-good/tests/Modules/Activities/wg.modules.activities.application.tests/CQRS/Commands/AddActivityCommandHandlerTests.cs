using System.Runtime.InteropServices.JavaScript;
using NSubstitute;
using Shouldly;
using wg.modules.activities.application.Clients;
using wg.modules.activities.application.Clients.Tickets;
using wg.modules.activities.application.Clients.Tickets.DTOs;
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
    public async Task HandleAsync_GivenExistingTicketWithStateForChangesAndNewDailyUserActivity_ShouldAddActivityByRepository()
    {
        //arrange
        var command = new AddActivityCommand(Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid(),
            "Test activity contet",
            ShortDateTimeProvider.Get(DateTime.Now.AddHours(-1)),
            ShortDateTimeProvider.Get(DateTime.Now), true);
        
        await _dailyUserActivityRepository
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
        await _dailyUserActivityRepository
            .AddAsync(Arg.Is<DailyUserActivity>(arg
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
    public async Task HandleAsync_GivenExistingTicketWithStateForChangesAndExistingDailyUserActivity_ShouldUpdateDailyUserActivity()
    {
        //arrange
        var dailyUserActivity = DailyUserActivityFactory.Get();
        var ticketId = Guid.NewGuid();
        
        dailyUserActivity.AddPaidActivity(Guid.NewGuid(), "Test content",
            Guid.NewGuid(), new DateTime(dailyUserActivity.Day.Value.Year, 
                dailyUserActivity.Day.Value.Month, dailyUserActivity.Day.Value.Day, 8,0,0),
            new DateTime(dailyUserActivity.Day.Value.Year, 
                dailyUserActivity.Day.Value.Month, dailyUserActivity.Day.Value.Day, 9,0,0));
        
        var command = new AddActivityCommand(Guid.NewGuid(), dailyUserActivity.UserId,ticketId,
            "Test activity content",
            new DateTime(dailyUserActivity.Day.Value.Year, dailyUserActivity.Day.Value.Month, 
                dailyUserActivity.Day.Value.Day, 9,0,0),
            new DateTime(dailyUserActivity.Day.Value.Year, dailyUserActivity.Day.Value.Month, 
                dailyUserActivity.Day.Value.Day, 10,0,0),
            true);
        
        
        _dailyUserActivityRepository
            .GetByDateForUser(command.TimeFrom.Date, command.UserId)
            .Returns(dailyUserActivity);
        
        _ticketsApiClient
            .IsAvailableForChangesTicketExists(Arg.Is<TicketIdDto>(arg => arg.TicketId == command.TicketId))
            .Returns(new TicketExistsDto()
            {
                Value = true
            });
        
        //act
        await Act(command);
        
        //assert
        await _dailyUserActivityRepository
            .Received(1)
            .UpdateAsync(dailyUserActivity);
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

    private readonly IDailyUserActivityRepository _dailyUserActivityRepository;
    private readonly ITicketsApiClient _ticketsApiClient;
    private readonly ICommandHandler<AddActivityCommand> _handler;

    public AddActivityCommandHandlerTests()
    {
        _dailyUserActivityRepository = Substitute.For<IDailyUserActivityRepository>();
        _ticketsApiClient = Substitute.For<ITicketsApiClient>();
        _handler = new AddActivityCommandHandler(_dailyUserActivityRepository,
            _ticketsApiClient);
    }
    #endregion
}