using System.Net;
using System.Net.Http.Json;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cms;
using Shouldly;
using wg.modules.companies.application.CQRS.Companies.Commands.AddCompany;
using wg.modules.companies.application.CQRS.Companies.Commands.UpdateCompany;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.application.CQRS.Tickets.Queries;
using wg.modules.tickets.application.DTOs;
using wg.tests.shared.Db;
using wg.tests.shared.Factories.Companies;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.companies.integration.tests;

[Collection("#1")]
public sealed class CompaniesControllerTests : BaseTestsController
{
    [Fact]
    public async Task GetAll_GivenPaginationFilters_ShouldReturnTicketsList()
    {
        //arrange
        var companies = CompanyFactory.Get(10);
        await CompaniesDbContext.Companies.AddRangeAsync(companies);
        await CompaniesDbContext.SaveChangesAsync();
        var query = new GetCompaniesQuery()
        {
            PageNumber = 1,
            PageSize = 10
        };
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add(nameof(GetCompaniesQuery.PageSize), query.PageSize.ToString());
        queryString.Add(nameof(GetCompaniesQuery.PageNumber), query.PageNumber.ToString());
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync($"companies-module/companies?{queryString.ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var pagination = GetPaginationMetaDataFromHeader(response);
        pagination.ShouldNotBeNull();
        var result = await response.Content.ReadFromJsonAsync<List<CompanyDto>>();
        result.Count.ShouldBe(10);
    }
    
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
    public async Task GetSlaTimeByEmployeeId_GivenExistingCompany_ShouldReturnSlaTimeDto()
    {
        //arrange
        var company = await AddCompanyAsync(true, false, false, false);
        var employee = company
            .Employees
            .Single();

        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<SlaTimeDto>($"companies-module/Companies/sla-time/{employee.Id.Value}");
        
        //assert
        result.Value.ShouldBe(company.SlaTime.Value);
    }
    
    [Fact]
    public async Task GetSlaTimeByEmployeeId_GivenNotExistingCompany_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync($"companies-module/Companies/sla-time/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetSlaTimeByEmployeeId_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"companies-module/Companies/sla-time/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
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

    [Fact]
    public async Task UpdateCompany_GivenValidArguments_ShouldReturn200OkStatusCodeAndChangedUserInDb()
    {
        //arrange
        var company = await AddCompanyAsync(false, false, false, false);
        var command = new UpdateCompanyCommand(Guid.Empty, "New name", TimeSpan.FromHours(10));
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"companies-module/companies/{company.Id.Value}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedCompany = await GetCompanyByIdAsync(company.Id);
        updatedCompany.Name.Value.ShouldBe(command.Name);
        updatedCompany.SlaTime.Value.ShouldBe(command.SlaTime);
    }
    
    [Fact]
    public async Task UpdateCompany_GivenNotExistingCompanyId_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new UpdateCompanyCommand(Guid.Empty, "New name", TimeSpan.FromHours(10));
        Authorize(Guid.NewGuid(), Role.Manager());
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"companies-module/companies/{Guid.NewGuid()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task UpdateCompany_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new UpdateCompanyCommand(Guid.Empty, "New name", TimeSpan.FromHours(10));
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"companies-module/companies/{Guid.NewGuid()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task UpdateCompany_AuthorizedAsUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        var command = new UpdateCompanyCommand(Guid.Empty, "New name", TimeSpan.FromHours(10));
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PutAsJsonAsync($"companies-module/companies/{Guid.NewGuid()}", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    private async Task<Company> AddCompanyAsync(bool withEmployee, bool withProject, bool withPlannedStart, bool withPlannedFinish)
    {
        var company = CompanyFactory.Get();
        if (withEmployee)
        {
            EmployeeFactory.GetInCompany(company);
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