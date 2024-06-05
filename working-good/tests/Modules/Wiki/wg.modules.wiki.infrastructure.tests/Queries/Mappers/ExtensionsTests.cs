using Shouldly;
using wg.modules.wiki.infrastructure.Queries.Mappers;
using wg.tests.shared.Factories.Wiki;
using Xunit;

namespace wg.modules.wiki.infrastructure.tests.Queries.Mappers;

public sealed class ExtensionsTests
{
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