using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.infrastructure.DAL;
using wg.tests.shared.Db;
using wg.tests.shared.Factories.Companies;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Integration;
using Xunit;
using State = wg.modules.tickets.domain.ValueObjects.Ticket.State;

namespace wg.modules.tickets.integration.tests;

[Collection("#1")]
public sealed class TicketsControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddTicket_GivenOnlyRequiredArgumentsAndAuthorized_ShouldReturn201StatusCodeWithResourceIdAndLocationHeaderAndAddTicketToDdb()
    {
        //arrange
        var ticket = await AddTicket();
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
        
        var addedTicket = await GetTicketByIdAsync((Guid)resourceId);
        addedTicket.ShouldNotBeNull();
        addedTicket.CreatedBy.Value.ShouldBe(userId);
        addedTicket.Number.Value.ShouldBe(ticket.Number + 1);
    }

    [Fact]
    public async Task AddTicket_GivenAllArgumentsWithFilledDbAndAuthorized_ShouldReturn201CreatedStatusCodeWithResourceIdAndLocationHeaderAndAddTicketToDb()
    {
        //arrange
        var existingTicket = await AddTicket();
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetEmployeeInCompany(company); 
        var project = ProjectFactory.GetInCompany(company, true, true);  
        var owner = OwnerFactory.Get();         
        var user = UserFactory.GetUserInOwner(owner, Role.Manager());
        user.Verify(DateTime.Now);
        owner.AddGroup(project.Id, project.Title);
        owner.AddUserToGroup(owner.Groups.Single().Id, user.Id);
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        await _companiesDbContext.Companies.AddAsync(company);
        await _companiesDbContext.SaveChangesAsync();
        var command = new AddTicketCommand(Guid.Empty, "My test ticket", "My test content", Guid.Empty,
            State.New(), true, employee.Id, user.Id, project.Id);
        Authorize(user.Id, user.Role);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("tickets-module/tickets/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);
        
        var addedTicket = await GetTicketByIdAsync((Guid)resourceId);
        addedTicket.ShouldNotBeNull();
        addedTicket.CreatedBy.ShouldBe(user.Id);
        addedTicket.Number.Value.ShouldBe(existingTicket.Number + 1);
    }

    [Fact]
    public async Task AddTicket_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddTicketCommand(Guid.Empty, "My test ticket", "My test content", Guid.Empty,
            State.New(), false, null, null, null);
        
        //act 
        var response = await HttpClient.PostAsJsonAsync("tickets-module/tickets/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddTicket_GivenNotExistingEmployeeIdAndAuthorized_ShouldReturn400BadRequest()
    {
        //arrange
        var command = new AddTicketCommand(Guid.Empty, "My test ticket", "My test content", Guid.Empty,
            State.New(), true, Guid.NewGuid(), null, null);
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("tickets-module/tickets/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_GivenExistingTicketIdWithMessageAndAuthorized_ShouldReturnTicketDtoWithMessageDto()
    {
        //arrange
        var ticket = await AddTicket(true);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetFromJsonAsync<TicketDto>($"tickets-module/tickets/{ticket.Id.Value}");
        
        //assert
        response.ShouldNotBeNull();
        response.Id.ShouldBe(ticket.Id.Value);
        response.Messages.ShouldNotBeNull();
        response.Messages.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetById_GivenNotExistingIdAndAuthorized_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync($"tickets-module/tickets/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetById_Unauthorized_ShouldReturn401Unauthorized()
    {
        //act
        var response = await HttpClient.GetAsync($"tickets-module/tickets/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    private async Task<Ticket> AddTicket(bool withMessage = false)
    {
        var ticket = TicketsFactory.GetOnlyRequired(State.Open());
        if (withMessage)
        {
            var message = MessagesFactory.Get().Single();
            ticket.AddMessage(message.Id, message.Sender, message.Subject, message.Content, 
                message.CreatedAt);
        }
        
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
    private readonly TicketsDbContext _ticketsDbContext;
    private readonly CompaniesDbContext _companiesDbContext;
    private readonly OwnerDbContext _ownerDbContext;
    
    public TicketsControllerTests()
    {
        _ticketsDbContext = TestAppDb.TicketsDbContext;
        _companiesDbContext = TestAppDb.CompaniesDbContext;
        _ownerDbContext = TestAppDb.OwnerDbContext;
    }
    #endregion
}