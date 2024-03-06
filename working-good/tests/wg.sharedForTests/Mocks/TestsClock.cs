using wg.shared.abstractions.Time;

namespace wg.sharedForTests.Mocks;

public sealed class TestsClock : IClock
{
    private readonly DateTime? _time;

    private TestsClock() { }

    private TestsClock(DateTime time)
        => _time = time;

    public static IClock Create()
        => new TestsClock();
    
    public static IClock Create(DateTime time)
        => new TestsClock(time);

    public DateTime Now()
        => _time ?? DateTime.Now;
}