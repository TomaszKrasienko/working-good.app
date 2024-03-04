using wg.shared.abstractions.Time;

namespace wg.shared.infrastructure.Time;

internal sealed class Clock : IClock
{
    public DateTime Now()
        => DateTime.Now;
}