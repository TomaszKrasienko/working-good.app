using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Events;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;

internal sealed class AddTicketCommandHandler(
    ITicketRepository ticketRepository,
    ICompaniesApiClient companiesApiClient,
    IOwnerApiClient ownerApiClient,
    IClock clock,
    IEventDispatcher eventDispatcher) : ICommandHandler<AddTicketCommand>
{
    public Task HandleAsync(AddTicketCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}