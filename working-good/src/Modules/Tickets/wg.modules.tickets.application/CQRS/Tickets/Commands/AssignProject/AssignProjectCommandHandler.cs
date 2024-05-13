using System.Data;
using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignProject;

internal sealed class AssignProjectCommandHandler(
    ITicketRepository ticketRepository,
    IOwnerApiClient ownerApiClient,
    ICompaniesApiClient companiesApiClient) : ICommandHandler<AssignProjectCommand>
{
    public async Task HandleAsync(AssignProjectCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.Id);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.Id);
        }

        if (ticket.AssignedUser is not null)
        {
            var isGroupMembershipExists = await ownerApiClient.IsMembershipExistsAsync(new GetMembershipDto()
            {
                UserId = ticket.AssignedUser,
                GroupId = command.ProjectId
            });

            if (!isGroupMembershipExists.Value)
            {
                throw new UserDoesNotBelongToGroupException(command.ProjectId, ticket.AssignedUser);
            }
        }

        if (ticket.AssignedEmployee is not null)
        {
            var isProjectForCompanyDto = await companiesApiClient.IsProjectForCompanyAsync(new EmployeeWithProjectDto()
            {
                EmployeeId = ticket.AssignedEmployee,
                ProjectId = command.ProjectId
            });

            if (!isProjectForCompanyDto.Value)
            {
                throw new InvalidProjectForEmployeeException(ticket.AssignedEmployee, command.ProjectId);
            }
        }

        if (ticket.AssignedEmployee is null && ticket.AssignedUser is null)
        {
            var isProjectActiveDto = await companiesApiClient.IsProjectActiveAsync(new ProjectIdDto()
            {
                Id = command.ProjectId
            });

            if (!isProjectActiveDto.Value)
            {
                throw new ActiveProjectNotFoundException(command.ProjectId);
            }
        }
        
        ticket.ChangeProject(command.ProjectId);
        await ticketRepository.UpdateAsync(ticket);
    }
}