using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;
using wg.modules.owner.application.CQRS.Owners.Commands.ChangeOwnerName;
using wg.modules.owner.application.CQRS.Owners.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.owner.infrastructure.DAL;
using wg.tests.shared.Db;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.owner.integration.tests;

[Collection("#1")]
public sealed class OwnerControllerTests : BaseTestsController
{
    [Fact]
    public async Task AddOwner_GivenAddOwnerCommand_ShouldReturn201CreatedStatusCodeWithResourceIdHeaderAndLocationHeaderAndAddOwnerToDb()
    {
        //arrange
        var command = new AddOwnerCommand(Guid.Empty, "owner_company_name");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/owner-module/owner/add", command);
     
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var owner = await GetOwnerAsync();
        owner.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task AddOwner_GivenAddOwnerCommandWithExistingOwner_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        await AddOwner();
        var command = new AddOwnerCommand(Guid.Empty, "owner_company_name");
        
        //act
        var response = await HttpClient.PostAsJsonAsync("/owner-module/owner/add", command);
     
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ChangeOwnerName_GivenNewNameAndAuthorizedManager_ShouldReturn204NoContentStatusCodeAndChangeNameInDb()
    {
        //arrange
        var owner = await AddOwner();
        Authorize(Guid.NewGuid(), Role.Manager());
        var command = new ChangeOwnerNameCommand("MyCompanyNewName");
            
        //act
        var result = await HttpClient.PatchAsJsonAsync("/owner-module/owner/change-name", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
        var changedOwner = await GetOwnerAsync();
        changedOwner!.Name.Value.ShouldBe(command.Name);
    }
    
    [Fact]
    public async Task ChangeOwnerName_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new ChangeOwnerNameCommand("MyCompanyNewName");
            
        //act
        var result = await HttpClient.PatchAsJsonAsync("/owner-module/owner/change-name", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task ChangeOwnerName_AuthorizedUser_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        var command = new ChangeOwnerNameCommand("MyCompanyNewName");
            
        //act
        var result = await HttpClient.PatchAsJsonAsync("/owner-module/owner/change-name", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task ChangeOwnerName_GivenEmptyNameAndAuthorizedManager_ShouldReturn400BadRequest()
    {
        //arrange
        var owner = await AddOwner();
        Authorize(Guid.NewGuid(), Role.Manager());
        var command = new ChangeOwnerNameCommand(string.Empty);
            
        //act
        var result = await HttpClient.PatchAsJsonAsync("/owner-module/owner/change-name", command);
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetOwner_ForExistingOwnerWithAuthorized_ShouldReturnOwnerDto()
    {
        //arrange
        await AddOwner();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetFromJsonAsync<OwnerDto>("owner-module/owner");
        
        //assert
        response.ShouldNotBeNull();
        response.ShouldBeOfType<OwnerDto>();
    }
    
    [Fact]
    public async Task GetOwner_ForNoExistingOwnerWithAuthorized_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync("owner-module/owner");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetOwner_ForUnauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync("owner-module/owner");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    private Task<Owner?> GetOwnerAsync()
        => _ownerDbContext
            .Owner
            .AsNoTracking()
            .FirstOrDefaultAsync();

    private async Task<Owner> AddOwner()
    {
        var owner = OwnerFactory.Get();
        await _ownerDbContext.Owner.AddAsync(owner);
        await _ownerDbContext.SaveChangesAsync();
        return owner;
    }
    
    #region arrange
    private readonly TestAppDb _testDb;
    private readonly OwnerDbContext _ownerDbContext;
    
    public OwnerControllerTests()
    {
        _testDb = new TestAppDb();
        _ownerDbContext = _testDb.OwnerDbContext;
    }

    public override void Dispose()
    {
        _testDb.Dispose();
    }
    #endregion
}