using System.Text.RegularExpressions;
using wg.shared.abstractions.Kernel.Exceptions;

namespace wg.shared.abstractions.Kernel.ValueObjects;

public sealed record Email
{
    private static readonly Regex Regex = new(
        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        RegexOptions.Compiled);

    public string Value { get; }

    internal Email(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new EmptyEmailException();
        if (!(Regex.IsMatch(value)))
            throw new InvalidEmailException(value);
        Value = value;
    }

    public static implicit operator string(Email email)
        => email.Value;

    public static implicit operator Email(string email)
        => new Email(email);
}