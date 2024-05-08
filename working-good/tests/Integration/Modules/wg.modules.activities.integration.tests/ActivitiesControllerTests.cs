using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.activities.application.CQRS.AddActivity;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.domain.Entities;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Integration;
using Xunit;
using State = wg.modules.tickets.domain.ValueObjects.Ticket.State;

namespace wg.modules.activities.integration.tests;

[Collection("#1")]
public sealed class ActivitiesControllerTests : BaseTestsController
{
    [Fact]
    public async Task Add_GivenValidArguments_ShouldReturn201CreatedStatusCodeWithResourceIdAndLocationHeaderAndAddToDdb()
    {
        //arrange
        var ticket = await AddTicket();
        var command = new AddActivityCommand(Guid.Empty, Guid.NewGuid(), ticket.Id, "Test Content",
            DateTime.Now, DateTime.Now.AddHours(1), true);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("activities-module/activities/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var dailyUserActivity = await ActivitiesDbContext
            .DailyUserActivities
            .Include(x => x.Activities)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        dailyUserActivity.ShouldNotBeNull();
        dailyUserActivity.Activities.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Add_GivenInvalidArguments_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new AddActivityCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), "Test Content",
            DateTime.Now, DateTime.Now.AddHours(1), true);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("activities-module/activities/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Add_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddActivityCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), "Test Content",
            DateTime.Now, DateTime.Now.AddHours(1), true);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("activities-module/activities/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    private async Task<Ticket> AddTicket()
    {
        var ticket = TicketsFactory.GetOnlyRequired(state: State.Open());
        await TicketsDbContext.Tickets.AddAsync(ticket);
        await TicketsDbContext.SaveChangesAsync();
        return ticket;
    }
}