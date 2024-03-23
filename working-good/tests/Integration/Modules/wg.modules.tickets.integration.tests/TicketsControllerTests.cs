using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;
using wg.modules.tickets.infrastructure.DAL;
using wg.sharedForTests.Db;
using wg.sharedForTests.Factories.Tickets;
using wg.sharedForTests.Integration;
using Xunit;
using State = wg.modules.tickets.domain.ValueObjects.Ticket.State;

namespace wg.modules.tickets.integration.tests;

public sealed class TicketsControllerTests : BaseTestsController, IDisposable
{
    [Fact]
    public async Task AddTicket_GivenOnlyRequiredArgumentsAndAuthor_ShouldReturn201StatusCodeAndAddTicket()
    {
        //arrange
        var existingTicket = TicketsFactory.GetOnlyRequired(State.Open());
        await _ticketsDbContext.Tickets.AddAsync(existingTicket);
        await _ticketsDbContext.SaveChangesAsync();
        var command = new AddTicketCommand(Guid.Empty, "My test ticket", "My test content", Guid.Empty,
            State.New(), false, null, null, null);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("tickets-module/tickets/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        response.Headers.TryGetValues("resource-id", out var value);
        value!.Single().ShouldNotBe(Guid.Empty.ToString());
        var ticket = await _ticketsDbContext
            .Tickets
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id.Equals(Guid.Parse(value!.Single())));
        ticket.ShouldNotBeNull();
        ticket.CreatedBy.Value.ShouldNotBe(Guid.Empty);
    }
    
    #region arrange

    private readonly TestAppDb _testAppDb;
    private readonly TicketsDbContext _ticketsDbContext;

    public TicketsControllerTests()
    {
        _testAppDb = new TestAppDb();
        _ticketsDbContext = _testAppDb.TicketsDbContext;
    }

    public override void Dispose()
    {
        _testAppDb.Dispose();
    }

    #endregion
}