namespace wg.modules.owner.domain.ValueObjects.User;

public sealed record State
{
    private static IEnumerable<string> AvailableStates = new[]
    {
        "Registered", "Active", "Deactivated"
    };

    public string Value { get; private set; }

    private State(string value)
    {
        Value = value;
    }

    internal static State Registered()
        => new State("Registered");

    internal static State Activate()
        => new State("Active");

    internal static State Deactivate()
        => new State("Deactivated");

    public static implicit operator string(State state)
        => state.Value;

    public static implicit operator State(string value)
        => new State(value);
}