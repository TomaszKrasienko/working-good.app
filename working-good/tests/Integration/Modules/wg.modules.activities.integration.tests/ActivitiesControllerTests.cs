// using System.Net;
// using System.Net.Http.Json;
// using Microsoft.EntityFrameworkCore;
// using Shouldly;
// using wg.modules.activities.application.CQRS.Activities.Commands.AddActivity;
// using wg.modules.activities.application.DTOs;
// using wg.modules.activities.domain.Entities;
// using wg.modules.owner.domain.ValueObjects.User;
// using wg.modules.tickets.domain.Entities;
// using wg.tests.shared.Factories.Activities;
// using wg.tests.shared.Factories.Tickets;
// using wg.tests.shared.Integration;
// using Xunit;
// using State = wg.modules.tickets.domain.ValueObjects.Ticket.State;
//
// namespace wg.modules.activities.integration.tests;
//
// [Collection("#1")]
// public sealed class ActivitiesControllerTests : BaseTestsController
// {
//     [Fact]
//     public async Task Add_GivenValidArguments_ShouldReturn201CreatedStatusCodeWithResourceIdAndLocationHeaderAndAddToDdb()
//     {
//         //arrange
//         var ticket = await AddTicket();
//         var command = new AddActivityCommand(Guid.Empty, Guid.NewGuid(), ticket.Id, "Test Content",
//             DateTime.Now, DateTime.Now.AddHours(1), true);
//         Authorize(Guid.NewGuid(), Role.User());
//         
//         //act
//         var response = await HttpClient.PostAsJsonAsync("activities-module/activities/add", command);
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.Created);
//         response.Headers.Location.ShouldNotBeNull();
//         
//         var resourceId = GetResourceIdFromHeader(response);
//         resourceId.ShouldNotBeNull();
//         resourceId.ShouldNotBe(Guid.Empty);
//
//         var dailyUserActivity = await ActivitiesDbContext
//             .DailyUserActivities
//             .Include(x => x.Activities)
//             .AsNoTracking()
//             .FirstOrDefaultAsync();
//         
//         dailyUserActivity.ShouldNotBeNull();
//         dailyUserActivity.Activities.Count.ShouldBe(1);
//     }
//
//     [Fact]
//     public async Task Add_GivenInvalidArguments_ShouldReturn400BadRequestStatusCode()
//     {
//         //arrange
//         var command = new AddActivityCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), "Test Content",
//             DateTime.Now, DateTime.Now.AddHours(1), true);
//         Authorize(Guid.NewGuid(), Role.User());
//         
//         //act
//         var response = await HttpClient.PostAsJsonAsync("activities-module/activities/add", command);
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
//     }
//
//     [Fact]
//     public async Task Add_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
//     {
//         //arrange
//         var command = new AddActivityCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), "Test Content",
//             DateTime.Now, DateTime.Now.AddHours(1), true);
//         
//         //act
//         var response = await HttpClient.PostAsJsonAsync("activities-module/activities/add", command);
//         
//         //assert
//         response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
//     }
//
//     [Fact]
//     public async Task GetById_GivenExistingId_ShouldReturn200OkStatusCodeWithActivityId()
//     {
//         //arrange
//         var ticket = await AddTicket();
//         var activity = await AddActivity();
//         Authorize(Guid.NewGuid(), Role.User());
//         
//         //act
//         var result = await HttpClient.GetFromJsonAsync<ActivityDto>($"activities-module/activities/{activity.Id.Value}");
//         
//         //assert
//         result.ShouldNotBeNull();
//         result.ShouldBeOfType<ActivityDto>();
//     }
//
//     [Fact]
//     public async Task GetById_GivenNotExistingId_ShouldReturn204NoContentStatusCode()
//     {
//         //arrange
//         Authorize(Guid.NewGuid(), Role.Manager());
//         
//         //act
//         var result = await HttpClient.GetAsync($"activities-module/activities/{Guid.NewGuid()}");
//         
//         //assert
//         result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
//     }
//
//     [Fact]
//     public async Task GetById_Unauthorized_ShouldBe401UnauthorizedStatusCode()
//     {
//         //act
//         var result = await HttpClient.GetAsync($"activities-module/activities/{Guid.NewGuid()}");
//         
//         //assert
//         result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
//     }
//     
//     private async Task<Ticket> AddTicket()
//     {
//         var ticket = TicketsFactory.GetOnlyRequired(state: State.Open());
//         await TicketsDbContext.Tickets.AddAsync(ticket);
//         await TicketsDbContext.SaveChangesAsync();
//         return ticket;
//     }
//
//     private async Task<Activity> AddActivity()
//     {
//         var activity = ActivityFactory.GetRandom(DateTime.Now.AddHours(-1), DateTime.Now);
//         await ActivitiesDbContext.Activities.AddAsync(activity);
//         await ActivitiesDbContext.SaveChangesAsync();
//         return activity;
//     } 
// }