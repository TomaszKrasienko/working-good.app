using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangePriority;

internal sealed class ChangePriorityCommandHandler(
    ITicketRepository ticketRepository,
    ICompaniesApiClient companiesApiClient,
    IClock clock) : ICommandHandler<ChangePriorityCommand>
{
    public async Task HandleAsync(ChangePriorityCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.TicketId);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.TicketId);
        }

        var ticketPriority = ticket.IsPriority;
        TimeSpan? slaTime = null;
        if (ticketPriority == false)
        {
            var result = await companiesApiClient
                .GetSlaTimeByEmployeeAsync(new EmployeeIdDto(ticket.AssignedEmployee));
            slaTime = result.Value;
        }
        ticket.ChangePriority(!ticketPriority, slaTime, clock.Now());
        await ticketRepository.UpdateAsync(ticket);
    }
}