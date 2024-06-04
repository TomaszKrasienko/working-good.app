using wg.modules.wiki.domain.ValueObjects.Note;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.wiki.domain.Entities;

public sealed class Note
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public Content Content { get; private set; }
    public Origin Origin { get; private set; }

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

    internal static Note Create(Guid id, string title, string content, string originType, string originId)
    {
        var note = Create(id, title, content);
        note.ChangeOrigin(originType, originId);
        return note;
    }

    private void ChangeOrigin(string type, string id)
        => Origin = new Origin(type, id);
    
    private void ChangeTitle(string title)
        => Title = title;

    private void ChangeContent(string content)
        => Content = content;
}