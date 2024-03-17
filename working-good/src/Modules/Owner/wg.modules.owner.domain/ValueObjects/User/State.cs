namespace wg.modules.owner.domain.ValueObjects.User;

public sealed record State
{
    private static IEnumerable<string> _availableStates = new[]
    {
        "Registered", "Active", "Deactivated"
    };

    public string Value { get; private set; }

    public State(string value)
    {
        //todo: Add conditions and tests
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