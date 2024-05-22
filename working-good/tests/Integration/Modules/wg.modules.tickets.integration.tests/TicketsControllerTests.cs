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
using wg.modules.tickets.application.CQRS.Tickets.Commands.UpdateTicket;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.ValueObjects.Ticket;
using wg.modules.tickets.infrastructure.DAL;
using wg.tests.shared.Db;
using wg.tests.shared.Factories.Companies;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Integration;
using Xunit;

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
     public async Task AddTicket_GivenValidArguments_ShouldReturn201StatusCodeWithResourceIdAndLocationHeaderAndAddTicketToDdb()
     {
         //arrange
         var owner = OwnerFactory.Get();         
         var user = UserFactory.GetInOwner(owner, Role.Manager());
         user.Verify(DateTime.Now);
         await _ownerDbContext.Owner.AddAsync(owner);
         await _ownerDbContext.SaveChangesAsync();
         
         var command = new AddTicketCommand(Guid.Empty, "My test ticket", "My test content", Guid.Empty);
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
         addedTicket.Number.Value.ShouldBe(1);
     }
     
     [Fact]
     public async Task AddTicket_GivenNotExistingCreatedById_ShouldReturn400BadRequest()
     {
         //arrange
         var command = new AddTicketCommand(Guid.Empty, "My test ticket", "My test content", Guid.Empty);
         Authorize(Guid.NewGuid(), Role.Manager());
         
         //act
         var response = await HttpClient.PostAsJsonAsync("tickets-module/tickets/add", command);
         
         //assert
         response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
     }

    [Fact]
    public async Task AddTicket_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
         //arrange
         var command = new AddTicketCommand(Guid.Empty, "My test ticket", "My test content", Guid.Empty);
         
         //act 
         var response = await HttpClient.PostAsJsonAsync("tickets-module/tickets/add", command);
         
         //assert
         response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateTicket_GivenExistingTicket_ShouldReturn200OkStatusCodeUpdateTicket()
    {
        //arrange
        var ticket = await AddTicket();
        Authorize(Guid.NewGuid(), Role.User());
        var command = new UpdateTicketCommand(Guid.Empty, "New subject", "New content");

        //act
        var response = await HttpClient.PatchAsJsonAsync($"tickets-module/tickets/{ticket.Id.Value}/update", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.Content.Value.ShouldBe(command.Content);
        updatedTicket.Subject.Value.ShouldBe(command.Subject);
    }

    [Fact]
    public async Task AssignEmployee_GivenExistingActiveEmployeeAndTicketWithoutProject_ShouldReturn200OkStatusCodeAndAssignEmployee()
    {
        //arrange
        var ticket = await AddTicket();
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetInCompany(company);
        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/employee/{employee.Id.Value}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.AssignedEmployee.Value.ShouldBe(employee.Id.Value);
    }

    [Fact]
    public async Task AssignEmployee_GivenExistingActiveEmployeeAndTicketWithProject_ShouldReturn200OkStatusCodeAndAssignEmployee()
    {
        //arrange
        var ticket = await AddTicket();
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetInCompany(company);
        var project = ProjectFactory.GetInCompany(company, true, false);
        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        
        ticket.ChangeProject(project.Id);
        TicketsDbContext.Update(ticket);
        await TicketsDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/employee/{employee.Id.Value}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.AssignedEmployee.Value.ShouldBe(employee.Id.Value);
    }

    [Fact]
    public async Task AssignEmployee_GivenNotExistingTicket_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{Guid.NewGuid()}/employee/{Guid.NewGuid()}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AssignEmployee_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{Guid.NewGuid()}/employee/{Guid.NewGuid()}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AssignUser_GivenExistingUserAndTicketWithProject_ShouldReturn200OkStatusCodeAndUpdateTicket()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        var group = GroupFactory.GetInOwner(owner);
        user.Verify(DateTime.Now);
        group.AddUser(user);
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();

        var ticket = await AddTicket();
        ticket.ChangeProject(group.Id);
        TicketsDbContext.Tickets.Update(ticket);
        await TicketsDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());

        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/user/{user.Id.Value}", null);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.AssignedUser.Value.ShouldBe(user.Id.Value);
    }

    [Fact]
    public async Task AssignUser_GivenExistingActiveUserAndTicketNotWithProject_ShouldReturn200OkStatusCodeAndUpdateTicket()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        user.Verify(DateTime.Now);
        
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();

        var ticket = await AddTicket();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act        
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/user/{user.Id.Value}", null);

        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.AssignedUser.Value.ShouldBe(user.Id.Value);
    }

    [Fact]
    public async Task AssignUser_GivenNotExistingTicket_ShouldReturn400BadRequestStatusCode()
    {
         //arrange
         Authorize(Guid.NewGuid(), Role.User());
             
         //act
         var response = await HttpClient.PatchAsync($"tickets-module/tickets/{Guid.NewGuid()}/user/{Guid.NewGuid()}", null);
         
         //assert
         response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AssignUser_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
         //act
         var response = await HttpClient.PatchAsync($"tickets-module/tickets/{Guid.NewGuid()}/user/{Guid.NewGuid()}", null);
         
         //assert
         response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangePriority_GivenNotPriorityTicket_ShouldReturn200OkStatusCodeAndUpdateTicket()
    {
        //arrange
        var ticket = await AddTicket();
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetInCompany(company);
        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        
        ticket.ChangeAssignedEmployee(employee.Id);
        TicketsDbContext.Tickets.Update(ticket);
        await TicketsDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/change-priority", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.IsPriority.Value.ShouldBeTrue();
    }

    [Fact]
    public async Task ChangePriority_GivenPriorityTicket_ShouldReturn200OkStatusCodeAndUpdateTicket()
    {
        //arrange
        var ticket = await AddTicket();
        ticket.ChangeAssignedEmployee(Guid.NewGuid());
        ticket.ChangePriority(true, TimeSpan.FromHours(9), DateTime.Now);

        TicketsDbContext.Tickets.Update(ticket);
        await TicketsDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/change-priority", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.IsPriority.Value.ShouldBeFalse();
    }

    [Fact]
    public async Task ChangePriority_GivenNotExistingTicket_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{Guid.NewGuid()}/change-priority", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ChangePriority_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{Guid.NewGuid()}/change-priority", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangeStatus_GivenExistingTicket_ShouldReturn200OkStatusCodeAndChangeStatus()
    {
        //arrange
        var ticket = await AddTicket();
        Authorize(Guid.NewGuid(), Role.User());
        var command = new ChangeTicketStatusCommand(Guid.Empty, Status.Open());
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"tickets-module/tickets/{ticket.Id.Value}/change-status", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.Status.Value.ShouldBe(Status.Open());
    }

    [Fact]
    public async Task ChangeStatus_GivenNotExistingTicket_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        var command = new ChangeTicketStatusCommand(Guid.Empty, Status.Open());
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"tickets-module/tickets/{Guid.NewGuid()}/change-status", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ChangeStatus_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new ChangeTicketStatusCommand(Guid.Empty, Status.Open());
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"tickets-module/tickets/{Guid.NewGuid()}/change-status", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AssignProject_GivenTicketWithoutEmployeeAndUser_ShouldReturn200OkStatusCodeAndUpdateTicket()
    {
        //arrange
        var ticket = await AddTicket();
        var company = CompanyFactory.Get();
        var project = ProjectFactory.GetInCompany(company, true, true);

        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        
        Authorize(Guid.NewGuid(), Role.User());
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/project/{project.Id.Value}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.ProjectId.ShouldBe(project.Id);
    }

    [Fact]
    public async Task AssignProject_GivenTicketWithEmployeeAndUser_ShouldReturn200OkStatusCodeAndUpdateTicket()
    {
        //arrange
        var ticket = await AddTicket();
        
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetInCompany(company);
        var project = ProjectFactory.GetInCompany(company, true, true);

        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();

        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        user.Verify(DateTime.Now);
        owner.AddGroup(project.Id, project.Title);
        owner.AddUserToGroup(project.Id, user.Id);

        await OwnerDbContext.Owner.AddAsync(owner);
        await OwnerDbContext.SaveChangesAsync();
        
        ticket.ChangeAssignedEmployee(employee.Id);
        ticket.ChangeAssignedUser(user.Id);
        TicketsDbContext.Update(ticket);
        await TicketsDbContext.SaveChangesAsync();
        
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{ticket.Id.Value}/project/{project.Id.Value}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedTicket = await GetTicketByIdAsync(ticket.Id);
        updatedTicket.ProjectId.ShouldBe(project.Id);
    }

    [Fact]
    public async Task AssignProject_GivenNotExistingTicket_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{Guid.NewGuid()}/project/{Guid.NewGuid()}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task AssignProject_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.PatchAsync($"tickets-module/tickets/{Guid.NewGuid()}/project/{Guid.NewGuid()}", null);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

     private async Task<Ticket> AddTicket(bool withMessage = false)
     {
         var ticket = TicketsFactory.Get();
         if (withMessage)
         {
             var message = MessagesFactory.Get();
             ticket.AddMessage(message.Id, message.Sender, message.Subject, message.Content, 
                 message.CreatedAt, false);
         }
         
         await _ticketsDbContext.Tickets.AddAsync(ticket);
         await _ticketsDbContext.SaveChangesAsync();
         return ticket;
     }

     private async Task<IEnumerable<Ticket>> AddMultipleTickets(int count)
     {
         var tickets = TicketsFactory.Get(count);
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