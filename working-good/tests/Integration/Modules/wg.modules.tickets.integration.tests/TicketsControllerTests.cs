using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.infrastructure.DAL;
using wg.tests.shared.Db;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Integration;
using Xunit;
using State = wg.modules.tickets.domain.ValueObjects.Ticket.State;

namespace wg.modules.tickets.integration.tests;

[Collection("#1")]
public sealed class TicketsControllerTests : BaseTestsController, IDisposable
{
    [Fact]
    public async Task AddTicket_GivenOnlyRequiredArgumentsAndAuthor_ShouldReturn201StatusCodeAndAddTicket()
    {
        //arrange
        var existingTicket = await AddTicket();
        var command = new AddTicketCommand(Guid.Empty, "My test ticket", "My test content", Guid.Empty,
            State.New(), false, null, null, null);
        var userId = Guid.NewGuid();
        Authorize(userId, Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("tickets-module/tickets/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);
        
        var ticket = await GetTicketByIdAsync((Guid)resourceId);
        ticket.ShouldNotBeNull();
        ticket.CreatedBy.Value.ShouldBe(userId);
        ticket.Number.Value.ShouldBe(existingTicket.Number + 1);
    }

    private async Task<Ticket> AddTicket()
    {
        var ticket = TicketsFactory.GetOnlyRequired(State.Open());
        await _ticketsDbContext.Tickets.AddAsync(ticket);
        await _ticketsDbContext.SaveChangesAsync();
        return ticket;
    }
    
    private Task<Ticket?> GetTicketByIdAsync(Guid id)
        =>  _ticketsDbContext
            .Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    
    
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