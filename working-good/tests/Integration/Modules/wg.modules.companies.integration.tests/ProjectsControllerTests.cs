using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.CQRS.Projects.Commands.AddProject;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.companies.integration.tests._Helpers;
using wg.modules.owner.domain.ValueObjects.User;
using wg.tests.shared.Factories.Companies;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.companies.integration.tests;

[Collection("#1")]
public sealed class ProjectsControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddProject_GivenExistingCompanyIdAndValidArgumentsForAuthorizedManager_ShouldReturn204CreatedStatusCodeAndAddToDb()
    {
        //arrange
        var company = CompanyFactory.Get();
        await _companiesDbContext.Companies.AddAsync(company);
        await _companiesDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.Manager());
        var command = new AddProjectCommand(Guid.Empty, Guid.Empty, "MyProject", "Description of project");
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/projects/companies/{company.Id.Value}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.TryGetValues("resource-id", out var values);
        values!.Single().ShouldNotBe(Guid.Empty.ToString());
        var project = await _companiesDbContext
            .Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Title == command.Title);
        project.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task AddProject_ForAuthorizedUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        var command = new AddProjectCommand(Guid.Empty, Guid.Empty, "MyProject", "Description of project");
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/projects/companies/{Guid.NewGuid()}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task AddProject_ForNotAuthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddProjectCommand(Guid.Empty, Guid.Empty, "MyProject", "Description of project");
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/projects/companies/{Guid.NewGuid()}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddProject_GivenNotExistingCompanyIdForAuthorizedManager_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.Manager());
        var command = new AddProjectCommand(Guid.Empty, Guid.Empty, "MyProject", "Description of project");
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/projects/companies/{Guid.NewGuid()}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    #region arrange
    private readonly TestDb _testDb;
    private readonly CompaniesDbContext _companiesDbContext;

    public ProjectsControllerTests()
    {
        _testDb = new TestDb();
        _companiesDbContext = _testDb.CompaniesDbContext;
    }

    public override void Dispose()
    {
        _testDb.Dispose();
    }
    #endregion
}