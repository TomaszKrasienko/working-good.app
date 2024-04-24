using wg.modules.tickets.domain.ValueObjects.Ticket;

namespace wg.modules.tickets.domain.Policies.Abstractions;

public interface IStatePolicy
{
    bool CanChangeState(State currentState);
}