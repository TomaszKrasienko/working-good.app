using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Messages.Commands.AddMessage;

internal sealed class AddMessageCommandHandler(
    IOwnerApiClient ownerApiClient,
    ITicketRepository ticketRepository,
    IClock clock) : ICommandHandler<AddMessageCommand>
{
    public Task HandleAsync(AddMessageCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}