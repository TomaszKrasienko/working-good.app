using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.CQRS.Projects.Commands.AddProject;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.owner.domain.ValueObjects.User;
using wg.tests.shared.Db;
using wg.tests.shared.Factories.Companies;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.companies.integration.tests;

[Collection("#1")]
public sealed class ProjectsControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddProject_GivenExistingCompanyIdAndAddProjectCommandAndAuthorizedManager_ShouldReturn204CreatedStatusCodeAndResourceIdHeaderAndLocationHeaderAndAddToDb()
    {
        //arrange
        var company = await AddCompanyAsync();
        Authorize(Guid.NewGuid(), Role.Manager());
        var command = new AddProjectCommand(Guid.Empty, Guid.Empty, "MyProject", "Description of project");
        
        //act
        var response = await HttpClient.PostAsJsonAsync($"companies-module/projects/companies/{company.Id.Value}/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);
        
        var project = await GetProjectByIdAsync((Guid)resourceId);
        project.ShouldNotBeNull();
        project.Title.Value.ShouldBe(command.Title);
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

    private async Task<Company> AddCompanyAsync()
    {
        var company = CompanyFactory.Get();
        await _companiesDbContext.Companies.AddAsync(company);
        await _companiesDbContext.SaveChangesAsync();
        return company;
    }
    
    private Task<Project> GetProjectByIdAsync(Guid id)
        =>  _companiesDbContext
        .Projects
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id.Equals(id))!;
    
    #region arrange
    private readonly TestAppDb _testDb;
    private readonly CompaniesDbContext _companiesDbContext;

    public ProjectsControllerTests()
    {
        _testDb = new TestAppDb();
        _companiesDbContext = _testDb.CompaniesDbContext;
    }

    public override void Dispose()
    {
        _testDb.Dispose();
    }
    #endregion
}