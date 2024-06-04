using Shouldly;
using wg.modules.wiki.domain.Exceptions;
using wg.tests.shared.Factories.Wiki;
using Xunit;

namespace wg.modules.wiki.domain.tests.Entities;

public sealed class SectionTests
{
    [Fact]
    public void ChangeParent_GivenParent_ShouldChangeParent()
    {
        //arrange
        var section = SectionsFactory.Get();
        var parentSection = SectionsFactory.Get();
        
        //act
        section.ChangeParent(parentSection);
        
        //assert
        section.Parent.ShouldBe(parentSection);
    }

    [Fact]
    public void AddNote_GivenNotExistingNote_ShouldAddToNotes()
    {
        //arrange
        var section = SectionsFactory.Get();
        var noteId = Guid.NewGuid();
        
        //act
        section.AddNote(noteId, "Title", "Content");
        
        //assert
        section.Notes.Any(x => x.Id.Equals(noteId)).ShouldBeTrue();   
    }

    [Fact]
    public void AddNote_GivenAlreadyExistingNoteId_ShouldThrowNoteAlreadyBelongsToSection()
    {
        //arrange
        var section = SectionsFactory.Get();
        var noteId = Guid.NewGuid();
        section.AddNote(noteId, "Title", "Content");
        
        //act
        var exception = Record.Exception(() => section.AddNote(noteId, "Title", "Content"));

        //assert
        exception.ShouldBeOfType<NoteAlreadyBelongsToSection>();
    }
}