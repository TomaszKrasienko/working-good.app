using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Events.Mappers;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Entities;
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
    public async Task HandleAsync(AddTicketCommand command, CancellationToken cancellationToken)
    {
        if (command.AssignedEmployee is not null &&
            !await companiesApiClient.IsEmployeeExists((Guid)command.AssignedEmployee))
        {
            throw new EmployeeDoesNotExistException((Guid)command.AssignedEmployee);
        }

        if (command.ProjectId is not null && command.AssignedEmployee is not null)
        {
            var dto = new EmployeeWithProjectDto()
            {
                ProjectId = (Guid)command.ProjectId,
                EmployeeId = (Guid)command.AssignedEmployee
            };
            if (!await companiesApiClient.IsProjectExists(dto))
            {
                throw new ProjectDoesNotExists((Guid)command.ProjectId, (Guid)command.AssignedEmployee);
            }
        }

        if (command.AssignedUser is not null &&
            !await ownerApiClient.IsUserExists((Guid)command.AssignedUser))
        {
            throw new UserDoesNotExistException((Guid)command.AssignedUser);
        }

        if (command.AssignedUser is not null && command.ProjectId is not null)
        {
            var dto = new UserInGroupDto()
            {
                GroupId = (Guid)command.ProjectId,
                UserId = (Guid)command.AssignedUser
            };
            if (!await ownerApiClient.IsUserInGroup(dto))
            {
                throw new UserDoesNotBelongToGroupException((Guid)command.ProjectId, (Guid)command.AssignedUser);
            }
        }

        DateTime? expirationDate = null;
        var now = clock.Now();
        if (command is { IsPriority: true, AssignedEmployee: not null })
        {
            var slaTimeSpan = await companiesApiClient.GetSlaTimeByEmployee((Guid)command.AssignedEmployee);
            expirationDate = now.Add(slaTimeSpan.SlaTime);
        }
        
        var maxNumber = await ticketRepository.GetMaxNumberAsync();
        var ticket = Ticket.Create(command.Id, maxNumber + 1, command.Subject, command.Content,
            now, command.CreatedBy, command.State, now, command.IsPriority,
            expirationDate, command.AssignedEmployee, command.AssignedUser, command.ProjectId);

        await ticketRepository.AddAsync(ticket);
        await eventDispatcher.PublishAsync(ticket.AsEvent());
    }
}