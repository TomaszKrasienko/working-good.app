using System;
using System.Collections.Generic;
using System.Linq;
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

    public static string New()
        => "New";

    public static string Open()
        => "Open";

    public static string InProgress()
        => "InProgress";

    public static string WaitingForResponse()
        => "WaitingForResponse";
    
    public static string Cancelled()
        => "Cancelled";

    public static string Done()
        => "Done";
}