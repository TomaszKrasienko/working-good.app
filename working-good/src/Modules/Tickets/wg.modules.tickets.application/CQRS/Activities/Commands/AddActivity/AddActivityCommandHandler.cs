using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Activities.Commands.AddActivity;

internal sealed class AddActivityCommandHandler(
    ITicketRepository ticketRepository,
    IOwnerApiClient ownerApiClient) : ICommandHandler<AddActivityCommand>
{
    
    
    public Task HandleAsync(AddActivityCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}