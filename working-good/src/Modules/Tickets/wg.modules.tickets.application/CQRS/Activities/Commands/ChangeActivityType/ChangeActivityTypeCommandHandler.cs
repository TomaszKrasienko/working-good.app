using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Activities.Commands.ChangeActivityType;

internal sealed class ChangeActivityTypeCommandHandler() : ICommandHandler<ChangeActivityTypeCommand>
{
    public Task HandleAsync(ChangeActivityTypeCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}