using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeProject;

internal sealed class ChangeProjectCommandHandler : ICommandHandler<ChangeProjectCommand>
{
    public Task HandleAsync(ChangeProjectCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}