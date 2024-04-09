using System;
using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects.Activity;

public record Note
{
    public string Value { get; }

    public Note(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyNoteException();
        }
        Value = value;
    }

    public static implicit operator Note(string value)
        => new Note(value);

    public static implicit operator string(Note note)
        => note.Value;
}