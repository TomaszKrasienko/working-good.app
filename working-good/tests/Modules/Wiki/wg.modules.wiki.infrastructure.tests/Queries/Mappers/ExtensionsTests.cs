using Shouldly;
using wg.modules.wiki.infrastructure.Queries.Mappers;
using wg.tests.shared.Factories.Wiki;
using Xunit;

namespace wg.modules.wiki.infrastructure.tests.Queries.Mappers;

public sealed class ExtensionsTests
{
    [Fact]
    public void AsDto_GivenSectionWithNotes_ShouldReturnSectionDtoWithNotesDto()
    {
        //arrange
        var section = SectionsFactory.Get();
        var note = NotesFactory.GetInSection(true, section);
        
        //act
        var result = section.AsDto();
        
        //assert
        result.Id.ShouldBe(section.Id.Value);
        result.Name.ShouldBe(section.Name.Value);
        result.Notes.ShouldNotBeEmpty();
    }
    
    [Fact]
    public void AsDto_GivenSectionWithoutNotes_ShouldReturnSectionDtoWithoutNotesDto()
    {
        //arrange
        var section = SectionsFactory.Get();
        
        //act
        var result = section.AsDto();
        
        //assert
        result.Id.ShouldBe(section.Id.Value);
        result.Name.ShouldBe(section.Name.Value);
        result.Notes.ShouldBeEmpty();
    }
    
    [Fact]
    public void AsDto_GivenNoteWithoutOrigin_ShouldReturnNoteDto()
    {
        //arrange
        var note = NotesFactory.Get(false);
        
        //act
        var result = note.AsDto();
        
        //assert
        result.Id.ShouldBe(note.Id.Value);
        result.Title.ShouldBe(note.Title.Value);
        result.Content.ShouldBe(note.Content.Value);
        result.OriginId.ShouldBeNull();
        result.OriginType.ShouldBeNull();
    }
    
    [Fact]
    public void AsDto_GivenNoteWithOrigin_ShouldReturnNoteDto()
    {
        //arrange
        var note = NotesFactory.Get(true);
        
        //act
        var result = note.AsDto();
        
        //assert
        result.Id.ShouldBe(note.Id.Value);
        result.Title.ShouldBe(note.Title.Value);
        result.Content.ShouldBe(note.Content.Value);
        result.OriginId.ShouldBe(note.Origin.Id);
        result.OriginType.ShouldBe(note.Origin.Type);
    }
}