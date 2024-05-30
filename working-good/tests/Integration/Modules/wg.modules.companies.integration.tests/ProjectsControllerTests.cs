using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.CQRS.Projects.Commands.AddProject;
using wg.modules.companies.application.CQRS.Projects.Commands.EditProject;
using wg.modules.companies.application.DTOs;
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
    public async Task GetById_GivenExistingProject_ShouldReturnProjectDto()
    {
        //arrange
        var company = CompanyFactory.Get();
        var project = ProjectFactory.GetInCompany(company, true, true);
        
        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<ProjectDto>($"companies-module/projects/{project.Id.Value}");
        
        //assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(project.Id.Value);
    }
    
    [Fact]
    public async Task GetById_GivenNotExistingProject_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync($"companies-module/Projects/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetById_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"companies-module/Projects/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task IsProjectActive_GivenActiveProject_ShouldReturnIsExistsDtoWithTrue()
    {
        //arrange
        var company = CompanyFactory.Get();
        var project = ProjectFactory.GetInCompany(company, true, true);

        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<IsExistsDto>($"companies-module/Projects/{project.Id.Value}/active");
        
        //assert
        result.Value.ShouldBeTrue();
    }
    
    [Fact]
    public async Task IsProjectActive_GivenNotActiveProject_ShouldReturnIsExistsDtoWithFalse()
    {
        //arrange
        var company = CompanyFactory.Get();
        var projectId = Guid.NewGuid();
        company.AddProject(projectId, "test", "test", DateTime.Now.AddMonths(-1),
            DateTime.Now.AddDays(-1));

        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<IsExistsDto>($"companies-module/Projects/{projectId}/active");
        
        //assert
        result.Value.ShouldBeFalse();
    }

    [Fact]
    public async Task IsProjectActive_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"companies-module/Employees/{Guid.NewGuid()}/active");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task IsActiveProjectForEmployeeExists_GivenExistingProjectInCompany_ShouldReturnIsExistsDtoWithTrue()
    {
        //arrange
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetInCompany(company);
        var project = ProjectFactory.GetInCompany(company, true, false);

        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<IsExistsDto>($"companies-module/projects/{project.Id.Value}/employee/{employee.Id.Value}/active");
        
        //assert
        result.Value.ShouldBeTrue();
    }
    
    [Fact]
    public async Task IsActiveProjectForEmployeeExists_GivenNotExistingProjectInCompany_ShouldReturnIsExistsDtoWithFalse()
    {
        //arrange
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetInCompany(company);
        var project = ProjectFactory.GetInCompany(company, true, false);

        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<IsExistsDto>($"companies-module/projects/{Guid.NewGuid()}/employee/{employee.Id.Value}/active");
        
        //assert
        result.Value.ShouldBeFalse();
    }
    
    [Fact]
    public async Task IsActiveProjectForEmployeeExists_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        
        //act
        var response = await HttpClient.GetAsync($"companies-module/projects/{Guid.NewGuid()}/employee/{Guid.NewGuid()}/active");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
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

    [Fact]
    public async Task EditProject_GivenExistingProjectId_ShouldReturn200OkStatusCodeAndChangeProject()
    {
        //arrange
        var company = await AddCompanyAsync();
        var project = ProjectFactory.GetInCompany(company, true, false);
        CompaniesDbContext.Companies.Update(company);
        await CompaniesDbContext.SaveChangesAsync();
        var command = new EditProjectCommand(Guid.Empty, "NewProjectTitle", "NewProjectDescription",
            DateTime.Now.AddDays(1), DateTime.Now.AddDays(30));
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"companies-module/projects/edit/{project.Id.Value}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedProject = await GetProjectByIdAsync(project.Id);
        updatedProject.Title.Value.ShouldBe(command.Title);
        updatedProject.Description.Value.ShouldBe(command.Description);
        updatedProject.PlannedStart.Value.ShouldBe((DateTime)command.PlannedStart!);
        updatedProject.PlannedFinish.Value.ShouldBe((DateTime)command.PlannedFinish!);
    }

    [Fact]
    public async Task EditProject_GivenNotExistingProjectId_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var company = await AddCompanyAsync();
        var command = new EditProjectCommand(Guid.Empty, "NewProjectTitle", "NewProjectDescription",
            DateTime.Now.AddDays(1), DateTime.Now.AddDays(30));
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"companies-module/projects/edit/{Guid.NewGuid()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task EditProject_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new EditProjectCommand(Guid.Empty, "NewProjectTitle", "NewProjectDescription",
            DateTime.Now.AddDays(1), DateTime.Now.AddDays(30));
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"companies-module/projects/edit/{Guid.NewGuid()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task EditProject_AuthorizedAsUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        var command = new EditProjectCommand(Guid.Empty, "NewProjectTitle", "NewProjectDescription",
            DateTime.Now.AddDays(1), DateTime.Now.AddDays(30));
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"companies-module/projects/edit/{Guid.NewGuid()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    private async Task<Company> AddCompanyAsync()
    {
        var company = CompanyFactory.Get();
        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        return company;
    }
    
    private Task<Project> GetProjectByIdAsync(Guid id)
        =>  CompaniesDbContext
        .Projects
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id.Equals(id))!;
}