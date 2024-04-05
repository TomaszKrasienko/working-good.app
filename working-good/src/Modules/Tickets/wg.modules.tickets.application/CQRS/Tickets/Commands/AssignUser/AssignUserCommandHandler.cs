using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignUser;

internal sealed class AssignUserCommandHandler(
    ITicketRepository ticketRepository,
    IOwnerApiClient ownerApiClient) : ICommandHandler<AssignUserCommand>
{
    public async Task HandleAsync(AssignUserCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.TicketId);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.TicketId);
        }

        var user = await ownerApiClient.GetUserByIdAsyncAsync(new UserIdDto(command.UserId));
        if (user is null)
        {
            throw new UserNotFoundException(command.UserId);
        }

        if (ticket.ProjectId is not null)
        {
            var isUserInProject = await ownerApiClient.IsUserInGroupAsync(new UserInGroupDto()
            {
                UserId = command.UserId,
                GroupId = ticket.ProjectId
            });
            if (!isUserInProject.Value)
            {
                throw new UserDoesNotBelongToGroupException(ticket.ProjectId, command.UserId);
            }
        }
        
        //ticket.ChangeAssignedUser(command.UserId);
    }
}