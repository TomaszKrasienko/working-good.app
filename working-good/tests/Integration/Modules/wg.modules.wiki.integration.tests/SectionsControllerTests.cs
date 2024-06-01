using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.wiki.core.DAL;
using wg.modules.wiki.core.Entities;
using wg.modules.wiki.core.Services.Commands;
using wg.tests.shared.Integration;

namespace wg.modules.wiki.integration.tests;

public sealed class SectionsControllerTests : BaseTestsController
{
    public async Task Add_GivenValidArguments_ShouldReturn201CreatedStatusCode()
    {
        //arrange
        var command = new AddSectionCommand(Guid.NewGuid(), "Section name", null);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("wiki-module/sections/add", command);
        
        //assert 
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var owner = await GetSectionAsync(command.Id);
        owner.ShouldNotBeNull();
    }

    private async Task<Section> GetSectionAsync(Guid id)
        => await WikiDbContext
            .Sections
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
}