using wg.modules.wiki.domain.ValueObjects.Note;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.wiki.domain.Entities;

public sealed class Note
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public Content Content { get; private set; }
    public EntityId SectionId { get; private set; }

    private Note(EntityId id)
    {
        Id = id;
    }

    internal static Note Create(Guid id, string title, string content)
    {
        var note = new Note(id);
        note.ChangeTitle(title);
        note.ChangeContent(content);
        return note;
    }
    
    private void ChangeTitle(string title)
        => Title = title;

    private void ChangeContent(string content)
        => Content = content;
}