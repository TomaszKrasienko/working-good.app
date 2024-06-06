using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.tickets.domain.Entities;
using wg.modules.wiki.application.CQRS.Notes.Commands;
using wg.modules.wiki.application.DTOs;
using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.ValueObjects.Note;
using wg.tests.shared.Factories.Companies;
using wg.tests.shared.Factories.Tickets;
using wg.tests.shared.Factories.Wiki;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.wiki.integration.tests;

[Collection("#1")]
public sealed class NoteControllerTests : BaseTestsController
{
    [Fact]
    public async Task GetById_GivenExistingNote_ShouldReturnNoteDto()
    {
        //arrange
        var section = await AddSection();
        var note = await AddNote(section);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act 
        var result = await HttpClient.GetFromJsonAsync<NoteDto>($"wiki-module/notes/{note.Id.Value}");
        
        //assert
        result.ShouldBeOfType<NoteDto>();
        result.Id.ShouldBe(note.Id.Value);
    }
    
    [Fact]
    public async Task GetById_GivenNotExistingNote_ShouldReturn204NoContentStatusCode()
    {
        //arrange
        Authorize(Guid.NewGuid(), Role.User());
        
        //act 
        var result = await HttpClient.GetAsync($"wiki-module/notes/{Guid.NewGuid()}");
        
        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }


    [Fact]
    public async Task GetById_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //act 
        var result = await HttpClient.GetAsync($"wiki-module/notes/{Guid.NewGuid()}");

        //assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Add_GivenExistingSectionWithoutOrigin_ShouldReturn201CreatedStatusCode()
    {
        //arrange
        var section = await AddSection();
        var command = new AddNoteCommand(Guid.Empty, "Test title", "Test content",
            Guid.Empty);
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync<AddNoteCommand>($"wiki-module/notes/section/{section.Id.Value}/add",
                command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var note = await GetNoteById(resourceId.Value);
        note.ShouldNotBeNull();
    }

    [Fact]
    public async Task Add_GivenExistingSectionWithClientOrigin_ShouldReturn201CreatedStatusCode()
    {
        //arrange
        var section = await AddSection();
        var client = CompanyFactory.Get();
        await CompaniesDbContext.Companies.AddAsync(client);
        await CompaniesDbContext.SaveChangesAsync();
        var command = new AddNoteCommand(Guid.Empty, "Test title", "Test content",
            Guid.Empty, Origin.Client(), client.Id.Value.ToString());
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync<AddNoteCommand>($"wiki-module/notes/section/{section.Id.Value}/add",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var note = await GetNoteById(resourceId.Value);
        note.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task Add_GivenExistingSectionWithTicketOrigin_ShouldReturn201CreatedStatusCode()
    {
        //arrange
        var section = await AddSection();
        var ticket = TicketsFactory.Get();
        await TicketsDbContext.Tickets.AddAsync(ticket);
        await TicketsDbContext.SaveChangesAsync();
        var command = new AddNoteCommand(Guid.Empty, "Test title", "Test content",
            Guid.Empty, Origin.Ticket(), ticket.Id.Value.ToString());
        Authorize(Guid.NewGuid(), Role.User());
        
        //act
        var response = await HttpClient.PostAsJsonAsync<AddNoteCommand>($"wiki-module/notes/section/{section.Id.Value}/add",
            command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();
        resourceId.ShouldNotBe(Guid.Empty);

        var note = await GetNoteById(resourceId.Value);
        note.ShouldNotBeNull();
    }

    private async Task<Section> AddSection()
    {
        var section = SectionsFactory.Get();
        await WikiDbContext.Sections.AddAsync(section);
        await WikiDbContext.SaveChangesAsync();
        return section;
    }
    
    private async Task<Note> AddNote(Section section)
    {
        var note = NotesFactory.GetInSection(true, section);
        WikiDbContext.Sections.Update(section);
        await WikiDbContext.SaveChangesAsync();
        return note;
    }

    private async Task<Note> GetNoteById(Guid id)
        => await WikiDbContext
            .Notes
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
}