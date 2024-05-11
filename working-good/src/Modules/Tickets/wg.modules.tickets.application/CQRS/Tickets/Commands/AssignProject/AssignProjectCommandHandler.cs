using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignProject;

internal sealed class AssignProjectCommandHandler : ICommandHandler<AssignProjectCommand>
{
    public Task HandleAsync(AssignProjectCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}