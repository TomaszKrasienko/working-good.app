using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignEmployee;

internal sealed class AssignEmployeeCommandHandler(
    ITicketRepository ticketRepository,
    ICompaniesApiClient companiesApiClient,
    IMessageBroker messageBroker) : ICommandHandler<AssignEmployeeCommand>
{
    public async Task HandleAsync(AssignEmployeeCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.TicketId);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.TicketId);
        }

        if (ticket.ProjectId is not null)
        {
            var isProjectForCompany = await companiesApiClient.IsProjectForCompanyAsync(new EmployeeWithProjectDto()
            {
                EmployeeId = command.EmployeeId,
                ProjectId = ticket.ProjectId
            });

            if (!isProjectForCompany.Value)
            {
                throw new InvalidProjectForEmployeeException(command.EmployeeId, ticket.ProjectId);
            }
        }
        else
        {
            var isActiveEmployeeExists = await companiesApiClient.IsActiveEmployeeExistsAsync(new EmployeeIdDto(command.EmployeeId));
            if (!isActiveEmployeeExists.Value)
            {
                throw new ActiveEmployeeNotFoundException(command.EmployeeId);
            }
        }
        
        ticket.ChangeAssignedEmployee(command.EmployeeId);
        await ticketRepository.UpdateAsync(ticket);

        var @event = new EmployeeAssigned(command.TicketId, ticket.Number, command.EmployeeId);
        await messageBroker.PublishAsync(@event);

    }
}