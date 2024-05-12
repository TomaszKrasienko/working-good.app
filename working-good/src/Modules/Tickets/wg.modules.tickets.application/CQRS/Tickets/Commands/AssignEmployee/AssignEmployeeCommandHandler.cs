using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.CQRS.Tickets.Commands.AssignProject;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignEmployee;

internal sealed class AssignEmployeeCommandHandler(
    ITicketRepository ticketRepository,
    ICompaniesApiClient companiesApiClient) : ICommandHandler<AssignEmployeeCommand>
{
    public Task HandleAsync(AssignEmployeeCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}