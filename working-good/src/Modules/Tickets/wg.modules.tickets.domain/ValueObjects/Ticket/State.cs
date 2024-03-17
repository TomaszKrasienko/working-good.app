using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects.Ticket;

public record State
{
    private static IEnumerable<string> _availableStates = new[]
    {
        "New", "Open", "InProgress", "WaitingForResponse", "Cancelled", "Done"
    };

    public string Value { get; private set; }
    public DateTime ChangeDate { get; private set; }

    public State(string value, DateTime changeDate)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyStateException();
        }

        if (!_availableStates.Contains(value))
        {
            throw new UnavailableStateException(value);
        }

        Value = value;
        ChangeDate = changeDate;
    }

    internal static string New()
        => "New";

    internal static string Open()
        => "Open";

    internal static string InProgress()
        => "InProgress";

    internal static string WaitingForResponse()
        => "WaitingForResponse";
    
    internal static string Cancelled()
        => "Cancelled";

    internal static string Done()
        => "Done";
}