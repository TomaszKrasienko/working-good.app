
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.tickets.application.CQRS.Messages.Commands.AddMessage;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.infrastructure.DAL;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Integration;
using Xunit;
using State = wg.modules.tickets.domain.ValueObjects.Ticket.State;

namespace wg.modules.tickets.integration.tests;

[Collection("#1")]
public sealed class MessagesControllerTests : BaseTestsController
{
    [Fact]
    public async Task GetById_GivenExistingIdAndAuthorized_ShouldReturnMessageDto()
    {
        //arrange
        var ticket = await AddTicketAsync();
        var message = await AddMessageToTicketAsync(ticket);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetFromJsonAsync<MessageDto>($"tickets-module/messages/{message.Id.Value}");
        
        //assert
        response.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetById_GivenNotExistingIdAndAuthorized_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync($"tickets-module/messages/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    
    [Fact]
    public async Task GetById_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {   
        //act
        var response = await HttpClient.GetAsync($"tickets-module/messages/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddMessage_GivenValidArgumentsAndAuthorized_ShouldReturn201CreatedStatusCodeWithResourceIdAndLocationHeader()
    {
        //arrange
        var ticket = await AddTicketAsync();
        var user = await AddUserAsync();
        var command = new AddMessageCommand(Guid.Empty, Guid.Empty, "My message", Guid.Empty);
        Authorize(user.Id, user.Role);
            
        //act
        var response = await HttpClient.PostAsJsonAsync<AddMessageCommand>($"tickets-module/messages/ticket/{ticket.Id.Value}/add",
                command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var message = await GetMessageByIdAsync((Guid)resourceId);
        message.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task AddMessage_GivenInvalidArgumentsAndAuthorized_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var ticket = await AddTicketAsync();
        var command = new AddMessageCommand(Guid.Empty, Guid.Empty, "My message", Guid.Empty);
        Authorize(Guid.NewGuid(), Role.User());
            
        //act
        var response = await HttpClient.PostAsJsonAsync<AddMessageCommand>($"tickets-module/messages/ticket/{ticket.Id.Value}/add",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task AddMessage_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddMessageCommand(Guid.Empty, Guid.Empty, "My message", Guid.Empty);
            
        //act
        var response = await HttpClient.PostAsJsonAsync<AddMessageCommand>($"tickets-module/messages/ticket/{Guid.NewGuid()}/add",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    private async Task<Ticket> AddTicketAsync()
    {
        var ticket = TicketsFactory.GetAll(State.New());
        await _ticketsDbContext.Tickets.AddAsync(ticket);
        await _ticketsDbContext.SaveChangesAsync();
        return ticket;
    }

    private async Task<Message> AddMessageToTicketAsync(Ticket ticket)
    {
        var message = MessagesFactory.Get().Single();
        ticket.AddMessage(message.Id, message.Sender, message.Subject, message.Content,
            message.CreatedAt);
        _ticketsDbContext.Tickets.Update(ticket);
        await _ticketsDbContext.SaveChangesAsync();
        return message;
    }

    private async Task<User> AddUserAsync()
    {
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        return user;
    }

    private Task<Message> GetMessageByIdAsync(Guid id)
        => _ticketsDbContext
            .Messages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    
    #region arrange
    private readonly TicketsDbContext _ticketsDbContext;
    private readonly CompaniesDbContext _companiesDbContext;
    private readonly OwnerDbContext _ownerDbContext;

    public MessagesControllerTests()
    {
        _ticketsDbContext = TestAppDb.TicketsDbContext;
        _companiesDbContext = TestAppDb.CompaniesDbContext;
        _ownerDbContext = TestAppDb.OwnerDbContext;
    }
    #endregion
}