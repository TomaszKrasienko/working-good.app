using wg.modules.owner.domain.Exceptions;

namespace wg.modules.owner.domain.ValueObjects.User;

public sealed record FullName
{
    public string FirstName { get; }
    public string LastName { get; }
    
    internal FullName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new EmptyFullNameFieldException(nameof(FirstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new EmptyFullNameFieldException(nameof(LastName));
        }
        FirstName = firstName;
        LastName = lastName;
    }

    public override string ToString()
        => $"{FirstName} {LastName}";
}