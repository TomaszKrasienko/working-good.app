using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Events.Mappers;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;

internal sealed class AddTicketCommandHandler(
    ITicketRepository ticketRepository,
    ICompaniesApiClient companiesApiClient,
    IOwnerApiClient ownerApiClient,
    IClock clock,
    IMessageBroker messageBroker) : ICommandHandler<AddTicketCommand>
{
    public async Task HandleAsync(AddTicketCommand command, CancellationToken cancellationToken)
    {
        DateTime? expirationDate = null;
        var now = clock.Now();
        if (command.AssignedEmployee is not null)
        {
            var companyDto = await companiesApiClient.GetCompanyByEmployeeIdAsync(new EmployeeIdDto((Guid)command.AssignedEmployee));
            if (companyDto is null)
            {
                throw new EmployeeDoesNotExistException((Guid)command.AssignedEmployee);
            }

            if (!IsProjectAssignedAndInProject(companyDto, command.ProjectId))
            {
                throw new ProjectDoesNotExists((Guid)command.ProjectId!, (Guid)command.AssignedEmployee);
            }
            
            if (IsPriority(command))
            {
                expirationDate = now.Add(companyDto.SlaTime);
            }
        }

        if (command.AssignedUser is not null)
        {
            var owner = await ownerApiClient.GetOwnerAsync();
            if (!IsUserExists(owner, command.AssignedUser))
            {
                throw new UserDoesNotExistException((Guid)command.AssignedUser);
            }

            if (!IsGroupAssignedAndUserInGroup(owner, command.AssignedUser, command.ProjectId))
            {
                throw new UserDoesNotBelongToGroupException((Guid)command.ProjectId!, (Guid)command.AssignedUser);
            }
        }
        
        var maxNumber = await ticketRepository.GetMaxNumberAsync();
        var ticket = Ticket.Create(command.Id, maxNumber + 1, command.Subject, command.Content,
            now, command.CreatedBy, command.State, now, command.IsPriority,
            expirationDate, command.AssignedEmployee, command.AssignedUser, command.ProjectId);

        await ticketRepository.AddAsync(ticket);
        await messageBroker.PublishAsync(ticket.AsEvent());
    }

    private bool IsProjectAssignedAndInProject(CompanyDto companyDto, Guid? projectId)
    {
        if (projectId is null) return true;
        return companyDto.Projects?
            .Any(p => p.Id.Equals((Guid)projectId)) 
            ?? false;
    }

    private bool IsPriority(AddTicketCommand command)
        => command.IsPriority;

    private bool IsUserExists(OwnerDto dto, Guid? userId)
        => dto.Users?
            .Any(u => u.Id.Equals(userId))
            ?? false;
    private bool IsGroupAssignedAndUserInGroup(OwnerDto ownerDto, Guid? userId, Guid? projectId)
    {
        if (projectId is null) return true;
        return ownerDto.Groups?
            .Any(g => g.Id.Equals(projectId) && g.Users.Any(x => x.Equals(userId)))
            ?? false;
    }
}