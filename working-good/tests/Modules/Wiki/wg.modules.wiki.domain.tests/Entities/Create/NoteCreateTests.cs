using Shouldly;
using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.Exceptions;
using wg.modules.wiki.domain.ValueObjects.Note;
using Xunit;

namespace wg.modules.wiki.domain.tests.Entities.Create;

public sealed class NoteCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnNoteWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var title = "Title";
        var content = "Content";
        
        //act
        var result = Note.Create(id, title, content);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Content.Value.ShouldBe(content);
    }
    
    [Fact]
    public void Create_GivenEmptyTitle_ShouldThrowEmptyNoteTitleException()
    {
        //act
        var exception = Record.Exception(() => Note.Create(Guid.NewGuid(), string.Empty, "Content"));
        
        //assert
        exception.ShouldBeOfType<EmptyNoteTitleException>();
    }
    
    [Fact]
    public void Create_GivenEmptyContent_ShouldThrowEmptyNoteSubjectException()
    {
        //act
        var exception = Record.Exception(() => Note.Create(Guid.NewGuid(), "Title", string.Empty));
        
        //assert
        exception.ShouldBeOfType<EmptyNoteContentException>();
    }

    [Fact]
    public void Create_GivenWithOrigin_ShouldReturnNoteWithFilledFields()
    {
         //arrange
         var id = Guid.NewGuid();
         var title = "Title";
         var content = "Content";
         var originType = Origin.Ticket();
         var originId = Guid.NewGuid().ToString();
         
         //act
         var result = Note.Create(id, title, content, originType, originId);
         
         //assert
         result.Id.Value.ShouldBe(id);
         result.Title.Value.ShouldBe(title);
         result.Content.Value.ShouldBe(content);
         result.Origin.Type.ShouldBe(originType);
         result.Origin.Id.ShouldBe(originId);
    }
    
    [Fact]
    public void Create_GivenWithUnavailableOriginType_ShouldThrowOriginTypeNoteAvailableException()
    {
        //act
        var exception = Record.Exception( () => Note.Create(Guid.NewGuid(), "Title", 
            "Content", "random", Guid.NewGuid().ToString()));
         
        //assert
        exception.ShouldBeOfType<OriginTypeNoteAvailableException>();
    }
    
    [Fact]
    public void Create_GivenEmptyOriginId_ShouldThrowEmptyOriginIdValueException()
    {
        //act
        var exception = Record.Exception( () => Note.Create(Guid.NewGuid(), "Title", 
            "Content", "random", Guid.NewGuid().ToString()));
         
        //assert
        exception.ShouldBeOfType<OriginTypeNoteAvailableException>();
    }
}