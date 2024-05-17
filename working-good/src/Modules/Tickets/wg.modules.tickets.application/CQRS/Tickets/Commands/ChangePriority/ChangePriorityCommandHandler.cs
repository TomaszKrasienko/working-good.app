using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangePriority;

internal sealed class ChangePriorityCommandHandler : ICommandHandler<ChangePriorityCommand>
{
    public Task HandleAsync(ChangePriorityCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}