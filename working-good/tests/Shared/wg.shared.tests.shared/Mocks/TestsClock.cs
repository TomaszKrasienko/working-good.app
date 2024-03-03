using wg.shared.abstractions.Time;

namespace wg.shared.tests.shared.Mocks;

public sealed class TestsClock : IClock
{
    private readonly DateTime? _time;

    private TestsClock() { }

    private TestsClock(DateTime time)
        => _time = time;

    internal static IClock Create()
        => new TestsClock();
    
    internal static IClock Create(DateTime time)
        => new TestsClock(time);

    public DateTime Now()
        => _time ?? DateTime.Now;
}