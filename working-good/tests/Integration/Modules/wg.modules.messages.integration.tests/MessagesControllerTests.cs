using System.Net;
using System.Net.Http.Json;
using Shouldly;
using wg.modules.messages.core.Services.Commands;
using wg.tests.shared.Factories.Companies;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.messages.integration.tests;

[Collection("#1")]
public sealed class MessagesControllerTests : BaseTestsController
{
    [Fact]
    public async Task Create_GivenExistingActiveEmployee_ShouldReturn202AcceptedStatusCode()
    {
        //arrange
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetInCompany(company);
        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();

        var command = new CreateMessage(employee.Email, "Subject", "Content", 1);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("messages-module/messages", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Accepted);
    } 
    
    [Fact]
    public async Task Create_GivenNotExistingActiveEmployee_ShouldReturn400BadRequestStatusCode()
    {
        //arrange

        var command = new CreateMessage("test@test.pl", "Subject", "Content", 1);
        
        //act
        var response = await HttpClient.PostAsJsonAsync("messages-module/messages", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    } 
}