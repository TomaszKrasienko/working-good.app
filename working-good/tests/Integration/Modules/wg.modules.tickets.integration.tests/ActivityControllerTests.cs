using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.application.CQRS.Activities.Commands.AddActivity;
using wg.modules.tickets.domain.Entities;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Integration;
using Xunit;
using State = wg.modules.tickets.domain.ValueObjects.Ticket.State;

namespace wg.modules.tickets.integration.tests;

public sealed class ActivityControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddActivity_GivenExistingTicketAndValidDates_ShouldReturn201CreatedStatusCodeWithResourceIdAndLocationHeader()
    {
        //arrange
        var ticket = await AddTicketAsync(State.Open());
        var user = await AddUserAsync();
        var command = new AddActivityCommand(Guid.Empty, Guid.Empty,
            DateTime.Now.AddHours(-2), DateTime.Now, "MyNote",
            true, user.Id);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"tickets-module/activities/ticket/{ticket.Id.Value}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var message = await GetActivityByIdAsync((Guid)resourceId);
        message.ShouldNotBeNull();
    }
    
    private async Task<Ticket> AddTicketAsync(string state)
    {
        var ticket = TicketsFactory.GetAll(state);
        await TicketsDbContext.Tickets.AddAsync(ticket);
        await TicketsDbContext.SaveChangesAsync();
        return ticket;
    }
    
    private async Task<User> AddUserAsync()
    {
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        user.Verify(DateTime.Now);
        await OwnerDbContext.Owner.AddAsync(owner);
        await OwnerDbContext.SaveChangesAsync();
        return user;
    }
    
    private Task<Activity> GetActivityByIdAsync(Guid id)
        => TicketsDbContext
            .Activities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
}

