using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.companies.application.CQRS.Companies.Commands.AddCompany;
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
public sealed class CompaniesControllerTests : BaseTestsController
{
    [Fact]
    public async Task GetById_GivenExistingIdAndAuthorized_ShouldReturnCompanyDto()
    {
        //arrange
        var company = await AddCompanyAsync(true, true, true, true);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<CompanyDto>($"companies-module/Companies/{company.Id.Value}");
        
        //assert
        result.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task GetById_GivenNotExistingIdAndAuthorized_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetAsync($"companies-module/Companies/{Guid.NewGuid().ToString()}");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetById_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var result = await HttpClient.GetAsync($"companies-module/Companies/{Guid.NewGuid().ToString()}");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddCompany_GivenAddCompanyCommandWithAuthorizedManager_ShouldReturn200OkStatusCodeWithResourceIdHeaderAndLocationHeaderAndAddCompanyToDb()
    {
        //arrange
        var command = new AddCompanyCommand(Guid.Empty, "NewCompanyTest", TimeSpan.FromHours(10), "test.pl");
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("companies-module/companies/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var company = await GetCompanyByIdAsync((Guid)resourceId);
        company.ShouldNotBeNull();
        company.Name.Value.ShouldBe(command.Name);
    }

    [Fact]
    public async Task AddCompany_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddCompanyCommand(Guid.Empty, "NewCompanyTest", TimeSpan.FromHours(10), "test.pl");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("companies-module/companies/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task AddCompany_AuthorizedUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        var command = new AddCompanyCommand(Guid.Empty, "NewCompanyTest", TimeSpan.FromHours(10), "test.pl");
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("companies-module/companies/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task AddCompany_GivenInvalidAddCompanyCommand_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new AddCompanyCommand(Guid.Empty, string.Empty, TimeSpan.FromHours(10), "test.pl");
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("companies-module/companies/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    private async Task<Company> AddCompanyAsync(bool withEmployee, bool withProject, bool withPlannedStart, bool withPlannedFinish)
    {
        var company = CompanyFactory.Get().Single();;
        if (withEmployee)
        {
            EmployeeFactory.GetInCompany(1, company);
        }

        if (withProject)
        {
            ProjectFactory.GetInCompany(company, withPlannedStart, withPlannedFinish);   
        }
        await CompaniesDbContext.Companies.AddAsync(company);
        await CompaniesDbContext.SaveChangesAsync();
        return company;
    }
    
    private Task<Company> GetCompanyByIdAsync(Guid id)
        => CompaniesDbContext
            .Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id))!;
}