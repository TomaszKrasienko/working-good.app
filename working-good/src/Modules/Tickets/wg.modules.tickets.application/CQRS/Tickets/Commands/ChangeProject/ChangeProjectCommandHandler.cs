using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeProject;

internal sealed class ChangeProjectCommandHandler(
    ITicketRepository ticketRepository,
    ICompaniesApiClient companiesApiClient) : ICommandHandler<ChangeProjectCommand>
{
    public async Task HandleAsync(ChangeProjectCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.TicketId);
        ticket.ChangeProject(command.ProjectId);
        await ticketRepository.UpdateAsync(ticket);
    }
}