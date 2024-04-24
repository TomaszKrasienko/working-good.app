using wg.modules.tickets.domain.Policies.Abstractions;
using wg.modules.tickets.domain.ValueObjects.Ticket;

namespace wg.modules.tickets.domain.Policies;

internal sealed class TicketStatePolicy : IStatePolicy
{
    internal static TicketStatePolicy Create()
        => new TicketStatePolicy();
    
    public bool CanChangeState(State currentState)
        => currentState != State.Cancelled() && currentState != State.Done();
}