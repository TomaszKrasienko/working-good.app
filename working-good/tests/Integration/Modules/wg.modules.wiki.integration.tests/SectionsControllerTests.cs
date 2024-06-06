using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.wiki.application.CQRS.Sections.Commands;
using wg.modules.wiki.application.CQRS.Sections.Commands.ChangeParent;
using wg.modules.wiki.application.DTOs;
using wg.modules.wiki.domain.Entities;
using wg.tests.shared.Factories.Wiki;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.wiki.integration.tests;

[Collection("#1")]
public sealed class SectionsControllerTests : BaseTestsController
{
    [Fact]
    public async Task GetById_GivenExistingSection_ShouldReturnSectionDto()
    {
        //arrange
        var section = await AddSection();
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var result = await HttpClient.GetFromJsonAsync<SectionDto>($"wiki-module/sections/{section.Id.Value}");
        
        //assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(section.Id.Value);
        result.Name.ShouldBe(section.Name.Value);
        result.Notes.ShouldBeEmpty();
    }
    
    [Fact]
    public async Task GetById_GivenNotExistingSection_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.GetAsync($"wiki-module/sections/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GetById_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act
        var response = await HttpClient.GetAsync($"wiki-module/sections/{Guid.NewGuid()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Add_GivenValidArguments_ShouldReturn201CreatedStatusCode()
    {
        //arrange
        var command = new AddSectionCommand(Guid.Empty, "Section name", null);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("wiki-module/sections/add", command);
        
        //assert 
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var section = await GetSectionAsync(resourceId.Value);
        section.ShouldNotBeNull();
    }

    [Fact]
    public async Task Add_GivenExistingSection_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var existingSection = await AddSection();
        var command = new AddSectionCommand(Guid.NewGuid(), existingSection.Name, Guid.NewGuid());
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("wiki-module/sections/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Add_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new AddSectionCommand(Guid.NewGuid(), "Section name", Guid.NewGuid());
        
        //act
        var response = await HttpClient.PostAsJsonAsync("wiki-module/sections/add", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangeParent_GivenExistingSections_ShouldReturn200OkStatusCodeAndUpdateSection()
    {
        //arrange
        var section = await AddSection();
        var parentSection = await AddSection();

        var command = new ChangeParentCommand(Guid.Empty, parentSection.Id);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"wiki-module/sections/{section.Id.Value}/change-parent", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedSection = await GetSectionAsync(section.Id);
        updatedSection.Parent.Id.ShouldBe(parentSection.Id);
    }

    [Fact]
    public async Task ChangeParent_GivenNotExistingSection_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var command = new ChangeParentCommand(Guid.Empty, null);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"wiki-module/sections/{Guid.NewGuid()}/change-parent", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ChangeParent_Unauthorized_ShouldReturn401UnauthorizedSatusCode()
    {
        //arrange
        var command = new ChangeParentCommand(Guid.Empty, null);
        
        //act
        var response = await HttpClient.PatchAsJsonAsync($"wiki-module/sections/{Guid.NewGuid()}/change-parent", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    private async Task<Section> AddSection()
    {
        var section = SectionsFactory.Get();
        await WikiDbContext.Sections.AddAsync(section);
        await WikiDbContext.SaveChangesAsync();
        return section;
    }

    private async Task<Section> GetSectionAsync(Guid id)
        => await WikiDbContext
            .Sections
            .AsNoTracking()
            .Include(x => x.Parent)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
}