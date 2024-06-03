using wg.modules.wiki.core.Exceptions;

namespace wg.modules.wiki.core.ValueObjects.Note;

public sealed record Content
{
    public string Value { get; private set; }

    public Content(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyNoteContentException();
        }
        Value = value;
    }

    public static implicit operator string(Content content)
        => content.Value;

    public static implicit operator Content(string value)
        => new Content(value);
}