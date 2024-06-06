using Bogus;
using wg.modules.wiki.domain.Entities;
using wg.modules.wiki.domain.ValueObjects.Note;

namespace wg.tests.shared.Factories.Wiki;

internal static class NotesFactory
{
    internal static Note GetInSection(bool withOrigin, Section section)
    {
        var note = Get(withOrigin);
        section.AddNote(note.Id, note.Title, note.Content, 
            note.Origin?.Type, note.Origin?.Id );
        return section.Notes.First(x => x.Id.Equals(note.Id));
    }

    internal static Note Get(bool withOrigin)
        => Get(1, withOrigin).Single();


    internal static List<Note> Get(int count, bool withOrigin)
        => withOrigin
            ? GetFullFaker().Generate(count)
            : GetBaseFaker().Generate(count);
    
    private static Faker<Note> GetBaseFaker()
        => new Faker<Note>().CustomInstantiator(v
            => Note.Create(
                Guid.NewGuid(),
                v.Lorem.Word(),
                v.Lorem.Sentence()));
    
    private static Faker<Note> GetFullFaker()
        => new Faker<Note>().CustomInstantiator(v
            => Note.Create(
                Guid.NewGuid(),
                v.Lorem.Word(),
                v.Lorem.Sentence(),
                v.PickRandom(Origin.AvailableOrigins),
                Guid.NewGuid().ToString()));
}