using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using wg.modules.tickets.domain.Exceptions;

namespace wg.modules.tickets.domain.ValueObjects.Ticket;

public record Status
{
    private static readonly IReadOnlyDictionary<string, bool> AvailableStatuses
        = new Dictionary<string, bool>()
        {
            {"New", true}, {"Open", true}, {"WaitingForResponse", true}, 
            {"Cancelled", false} , {"Done", false}
        };

    public static IEnumerable<string> AvailableForChangesStatuses => AvailableStatuses
        .Where(x => x.Value is true)
        .Select(x => x.Key)
        .ToImmutableList();
    
    public string Value { get; }
    public DateTime ChangeDate { get; }

    public Status(string value, DateTime changeDate)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyStatusException();
        }

        if (!AvailableStatuses.Keys.Contains(value))
        {
            throw new UnavailableStatusException(value);
        }

        Value = value;
        ChangeDate = changeDate;
    }

    public static string New()
        => "New";

    public static string Open()
        => "Open";
    
    public static string WaitingForResponse()
        => "WaitingForResponse";
    
    public static string Cancelled()
        => "Cancelled";

    public static string Done()
        => "Done";

    public static bool operator ==(Status state, string stateValue)
        => state?.Value == stateValue;

    public static bool operator !=(Status state, string stateValue) 
        => state?.Value != stateValue;
}