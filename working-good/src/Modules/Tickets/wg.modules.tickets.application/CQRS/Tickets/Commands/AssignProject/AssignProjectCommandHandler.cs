using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignProject;

internal sealed class AssignProjectCommandHandler(
    ITicketRepository ticketRepository,
    IOwnerApiClient ownerApiClient) : ICommandHandler<AssignProjectCommand>
{
    public Task HandleAsync(AssignProjectCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}