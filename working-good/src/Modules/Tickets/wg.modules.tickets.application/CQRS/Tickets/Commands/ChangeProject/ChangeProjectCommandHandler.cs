using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.ChangeProject;

internal sealed class ChangeProjectCommandHandler(
    ITicketRepository ticketRepository,
    ICompaniesApiClient companiesApiClient,
    IOwnerApiClient ownerApiClient) : ICommandHandler<ChangeProjectCommand>
{
    public async Task HandleAsync(ChangeProjectCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.TicketId);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.TicketId);
        }

        if (ticket.AssignedUser is not null)
        {
            var isUserBelongToGroup = await ownerApiClient.IsMembershipExistsAsync(new GetMembershipDto()
            {
                UserId = ticket.AssignedUser,
                GroupId = command.ProjectId
            });
            
            if (!isUserBelongToGroup.Value)
            {
                throw new UserDoesNotBelongToGroupException(command.ProjectId, ticket.AssignedUser);
            }
        }

        if (ticket.AssignedEmployee is not null)
        {
            var isProjectForEmployee = await companiesApiClient.IsProjectForCompanyAsync(new EmployeeWithProjectDto()
            {
                EmployeeId = ticket.AssignedEmployee,
                ProjectId = command.ProjectId
            });

            if (!isProjectForEmployee.Value)
            {
                throw new InvalidProjectForEmployeeException(ticket.AssignedEmployee, command.ProjectId);
            }
        }
        else
        {
            var isActiveProjectExists = await companiesApiClient.IsProjectActiveAsync(new ProjectIdDto()
            {
                Id = command.ProjectId
            });
            
            if (!isActiveProjectExists.Value)
            {
                throw new ActiveProjectNotFoundException(command.ProjectId);
            }
        }

        ticket.ChangeProject(command.ProjectId);
        await ticketRepository.UpdateAsync(ticket);
    }
}