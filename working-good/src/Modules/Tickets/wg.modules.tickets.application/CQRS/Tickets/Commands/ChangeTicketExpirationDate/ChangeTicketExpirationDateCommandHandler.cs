using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeTicketExpirationDate;

internal sealed class ChangeTicketExpirationDateCommandHandler(
    ITicketRepository ticketRepository,
    ICompaniesApiClient companiesApiClient) : ICommandHandler<ChangeTicketExpirationDateCommand>
{
    public async Task HandleAsync(ChangeTicketExpirationDateCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.Id);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.Id);
        }

        TimeSpan? limitTime = null;
        if (ticket.AssignedEmployee is not null)
        {
            var slaTime = await companiesApiClient
                .GetSlaTimeByEmployeeAsync(new EmployeeIdDto(ticket.AssignedEmployee));
            limitTime = slaTime.Value;
        }
        
        ticket.ChangeExpirationDate(command.ExpirationDate, limitTime);
        await ticketRepository.UpdateAsync(ticket);
    }
}