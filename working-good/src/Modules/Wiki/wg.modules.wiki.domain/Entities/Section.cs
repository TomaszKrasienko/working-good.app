using wg.modules.wiki.domain.Exceptions;
using wg.modules.wiki.domain.ValueObjects.Section;
using wg.shared.abstractions.Kernel.Types;

namespace wg.modules.wiki.domain.Entities;

public sealed class Section : AggregateRoot
{
    private readonly List<Note> _notes = new List<Note>();
    public EntityId Id { get; }
    public Name Name { get; private set; }
    public Section Parent { get; private set; }
    public IReadOnlyList<Note> Notes => _notes;

    private Section(EntityId id)
    {
        Id = id;
    }

    public static Section Create(Guid id, string name, Section parent = null)
    {
        var section = new Section(id);
        section.ChangeName(name);
        if (parent is not null)
        {
            section.ChangeParent(parent);
        }

        return section;
    }

    private void ChangeName(string name)
        => Name = name;

    public void ChangeParent(Section parent)
        => Parent = parent;

    public void AddNote(Guid id, string title, string content,
        string originType = null, string originId = null)
    {
        if (_notes.Any(x => x.Id.Equals(id)))
        {
            throw new NoteAlreadyBelongsToSection(id);
        }

        if (string.IsNullOrWhiteSpace(originType) || string.IsNullOrWhiteSpace(originId))
        {
            var note = Note.Create(id, title, content);
            _notes.Add(note);
        }
        else
        {
            var note = Note.Create(id, title, content, originType, originId);
            _notes.Add(note);
        }
    }
}