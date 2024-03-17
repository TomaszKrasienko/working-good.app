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

    internal static State New(DateTime time)
        => new State("New", time);
    
    internal static State Open(DateTime time)
        => new State("Open", time);
    
    internal static State InProgress(DateTime time)
        => new State("InProgress", time);
    
    internal static State WaitingForResponse(DateTime time)
        => new State("WaitingForResponse", time);
    
    internal static State Cancelled(DateTime time)
        => new State("Cancelled", time);
    
    internal static State Done(DateTime time)
        => new State("Done", time);
}