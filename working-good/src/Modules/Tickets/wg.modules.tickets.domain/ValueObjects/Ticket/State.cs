using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects.Ticket;

public record State
{
    public static readonly IReadOnlyDictionary<string, bool> AvailableStates
        = new Dictionary<string, bool>()
        {
            {"New", true}, {"Open", true}, {"InProgress", true}, {"WaitingForResponse", true}, 
            {"Cancelled", false} , {"Done", false}
        };

    public static IEnumerable<string> AvailableChangesStates => AvailableStates
        .Where(x => x.Value is true)
        .Select(x => x.Key)
        .ToImmutableList();
    
    public string Value { get; private set; }
    public DateTime ChangeDate { get; private set; }

    public State(string value, DateTime changeDate)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyStateException();
        }

        if (!AvailableStates.Keys.Contains(value))
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

    public static bool operator ==(State state, string stateValue)
        => state?.Value == stateValue;

    public static bool operator !=(State state, string stateValue) 
        => state?.Value != stateValue;
}