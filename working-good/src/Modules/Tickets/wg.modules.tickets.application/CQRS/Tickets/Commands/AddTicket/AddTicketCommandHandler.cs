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
        //Todo: Should I get company with all properties?
        //Todo: Here I can get company with projects and employees
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
                throw new ProjectDoesNotExists((Guid)command.ProjectId, (Guid)command.AssignedEmployee);
            }
            
            if (IsPriority(command))
            {
                expirationDate = now.Add(companyDto.SlaTime);
            }
        }
        
        if (command.AssignedUser is not null)
        {
            var isExistsDto = await ownerApiClient.IsUserExistsAsync(new UserIdDto((Guid)command.AssignedUser));
            if (!isExistsDto.Value)
            {
                throw new UserDoesNotExistException((Guid)command.AssignedUser);
            }
        }

        if (command.AssignedUser is not null && command.ProjectId is not null)
        {
            var dto = new UserInGroupDto()
            {
                GroupId = (Guid)command.ProjectId,
                UserId = (Guid)command.AssignedUser
            };
            var isInGroupDto = await ownerApiClient.IsUserInGroupAsync(dto);
            if (!isInGroupDto.Value)
            {
                throw new UserDoesNotBelongToGroupException((Guid)command.ProjectId, (Guid)command.AssignedUser);
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
        return companyDto.Projects.Any(p => p.Id.Equals((Guid)projectId));
    }

    private bool IsPriority(AddTicketCommand command)
        => command.IsPriority;
}