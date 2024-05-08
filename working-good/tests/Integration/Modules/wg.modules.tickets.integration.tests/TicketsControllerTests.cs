using System.Net;
using System.Net.Http.Json;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shouldly;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;
using wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketState;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
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
    public async Task GetAll_GivenPaginationFilters_ShouldReturnTicketsList()
    {
        //arrange
        var tickets = await AddMultipleTickets(40);
        var query = new GetTicketsQuery
        {
            PageNumber = 1,
            PageSize = 10
        };
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add(nameof(GetTicketsQuery.PageSize), query.PageSize.ToString());
        queryString.Add(nameof(GetTicketsQuery.PageNumber), query.PageNumber.ToString());
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync($"tickets-module/tickets?{queryString.ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var pagination = GetPaginationMetaDataFromHeader(response);
        pagination.ShouldNotBeNull();
        var result = await response.Content.ReadFromJsonAsync<List<TicketDto>>();
        result.Count.ShouldBe(10);
    }
    
    [Fact]
    public async Task GetAll_GivenPaginationFiltersAndTicketNumber_ShouldReturnTicketsList()
    {
        //arrange
        var tickets = (await AddMultipleTickets(30)).ToList();
        var ticketNumber = tickets.First().Number;
        var selectedTicket = tickets.Where(x => x.Number == ticketNumber).ToList();
        var query = new GetTicketsQuery
        {
            PageNumber = 1,
            PageSize = selectedTicket.Count,
            TicketNumber = ticketNumber
        };
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add(nameof(GetTicketsQuery.PageSize), query.PageSize.ToString());
        queryString.Add(nameof(GetTicketsQuery.PageNumber), query.PageNumber.ToString());
        queryString.Add(nameof(GetTicketsQuery.TicketNumber), query.TicketNumber.ToString());
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<List<TicketDto>>($"tickets-module/tickets?{queryString.ToString()}");
        
        //assert
        result.Count.ShouldBe(selectedTicket.Count);
    }
    
    [Fact]
    public async Task GetAll_GivenPaginationFiltersForEmptyTickets_ShouldReturnNoContentStatusCode()
    {
        //arrange
        var query = new GetTicketsQuery
        {
            PageNumber = 1,
            PageSize = 10
        };
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add(nameof(GetTicketsQuery.PageSize), query.PageSize.ToString());
        queryString.Add(nameof(GetTicketsQuery.PageNumber), query.PageNumber.ToString());
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync($"tickets-module/tickets?{queryString.ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetAll_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var query = new GetTicketsQuery
        {
            PageNumber = 1,
            PageSize = 10
        };
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add(nameof(GetTicketsQuery.PageSize), query.PageSize.ToString());
        queryString.Add(nameof(GetTicketsQuery.PageNumber), query.PageNumber.ToString());
        
        //act
        var response = await HttpClient.GetAsync($"tickets-module/tickets?{queryString.ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
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
    
    [Fact]
    public async Task AddTicket_GivenOnlyRequiredArgumentsAndAuthorized_ShouldReturn201StatusCodeWithResourceIdAndLocationHeaderAndAddTicketToDdb()
    {
        //arrange
        var ticket = await AddTicket();
        var owner = OwnerFactory.Get();         
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        user.Verify(DateTime.Now);
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        
        var command = new AddTicketCommand(Guid.Empty, "My test ticket", "My test content", Guid.Empty,
            State.New(), false, null, null, null);
        Authorize(user.Id, Role.User());
        
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
        addedTicket.CreatedBy.Value.ShouldBe(user.Email.Value);
        addedTicket.Number.Value.ShouldBe(ticket.Number + 1);
    }

    [Fact]
    public async Task AddTicket_GivenAllArgumentsWithFilledDbAndAuthorized_ShouldReturn201CreatedStatusCodeWithResourceIdAndLocationHeaderAndAddTicketToDb()
    {
        //arrange
        var existingTicket = await AddTicket();
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetInCompany(company); 
        var project = ProjectFactory.GetInCompany(company, true, true);  
        var owner = OwnerFactory.Get();         
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        user.Verify(DateTime.Now);
        owner.AddGroup(project.Id, project.Title);
        owner.AddUserToGroup(owner.Groups.Single().Id, user.Id);
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        await _companiesDbContext.Companies.AddAsync(company);
        await _companiesDbContext.SaveChangesAsync();
        var command = new AddTicketCommand(Guid.Empty, "My test ticket", "My test content", user.Id,
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
        addedTicket.CreatedBy.Value.ShouldBe(user.Email.Value);
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
    public async Task AssignUser_GivenExistingTicketIdAndUserId_ShouldReturn200OkStatusCodeAndUpdateTicket()
    {
        //arrange
        var ticket = await AddTicket();
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        user.Verify(DateTime.Now);
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/assign/user/{user.Id.Value}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.AssignedUser.Value.ShouldBe(user.Id.Value);
    }
    
    [Fact]
    public async Task AssignUser_GivenExistingTicketIdAndUserIdAndProjectId_ShouldReturn200OkStatusCodeAndUpdateTicket()
    {
        //arrange
        var ticket = await AddTicket();
        var owner = OwnerFactory.Get();
        var group = GroupFactory.GetInOwner(owner);
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        user.Verify(DateTime.Now);
        group.AddUser(user);
        ticket.ChangeProject(group.Id);
        _ticketsDbContext.Tickets.Update(ticket);
        await _ticketsDbContext.SaveChangesAsync();
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/assign/user/{user.Id.Value}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.AssignedUser.Value.ShouldBe(user.Id.Value);
    }
    
    [Fact]
    public async Task AssignUser_GivenUserNotInProject_ShouldReturn400BadRequestStatusCodeAndNotUpdateTicket()
    {
        //arrange
        var ticket = await AddTicket();
        var owner = OwnerFactory.Get();
        var group = GroupFactory.GetInOwner(owner);
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        ticket.ChangeProject(group.Id);
        _ticketsDbContext.Tickets.Update(ticket);
        await _ticketsDbContext.SaveChangesAsync();
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/assign/user/{user.Id.Value}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.AssignedUser.ShouldBeNull();
    }
    
    [Fact]
    public async Task AssignUser_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{Guid.NewGuid()}/assign/user/{Guid.NewGuid()}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangeTicketState_GivenValidArguments_ShouldReturn200OkStatusCodeAndChangedTicketInDb()
    {
        //arrange
        var ticket = await AddTicket();
        Authorize(Guid.NewGuid(), Role.User());
        var command = new ChangeTicketStateCommand(Guid.Empty, State.InProgress());
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"/tickets-module/tickets/{ticket.Id.Value}/change-state", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.State.Value.ShouldBe(command.State);
    }

    private async Task<Ticket> AddTicket(bool withMessage = false)
    {
        var ticket = TicketsFactory.GetOnlyRequired(state: State.Open()).Single();
        if (withMessage)
        {
            var message = MessagesFactory.Get();
            ticket.AddMessage(message.Id, message.Sender, message.Subject, message.Content, 
                message.CreatedAt);
        }
        
        await _ticketsDbContext.Tickets.AddAsync(ticket);
        await _ticketsDbContext.SaveChangesAsync();
        return ticket;
    }

    private async Task<IEnumerable<Ticket>> AddMultipleTickets(int count)
    {
        var tickets = TicketsFactory.GetOnlyRequired(count:count);
        await _ticketsDbContext.Tickets.AddRangeAsync(tickets);
        await _ticketsDbContext.SaveChangesAsync();
        return tickets;
    }
    
    private Task<Ticket> GetTicketByIdAsync(Guid id)
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