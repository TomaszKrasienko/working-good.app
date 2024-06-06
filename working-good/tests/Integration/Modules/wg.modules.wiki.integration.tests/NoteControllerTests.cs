using System.Net.Http.Json;
using Shouldly;
using wg.modules.owner.domain.ValueObjects.User;
using wg.modules.wiki.application.DTOs;
using wg.modules.wiki.domain.Entities;
using wg.tests.shared.Factories.Wiki;
using wg.tests.shared.Integration;
using Xunit;

namespace wg.modules.wiki.integration.tests;

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
}