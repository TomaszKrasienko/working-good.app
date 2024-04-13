using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignUser;

internal sealed class AssignUserCommandHandler(
    ITicketRepository ticketRepository,
    IOwnerApiClient ownerApiClient,
    IClock clock) : ICommandHandler<AssignUserCommand>
{
    public async Task HandleAsync(AssignUserCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.TicketId);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.TicketId);
        }

        if (ticket.ProjectId is null)
        {
            var user = await ownerApiClient.GetActiveUserByIdAsync(new UserIdDto(command.UserId));
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }
        }
        else
        {
            var owner = await ownerApiClient.GetOwnerAsync(new GetOwnerDto());
            if (!owner.Users.Any(u => u.Id.Equals(command.UserId)))
            {
                throw new UserNotFoundException(command.UserId);
            }
            
            if (!owner.Groups.Any(g => g.Id.Equals(ticket.ProjectId) && g.Users.Any(x => x.Equals(command.UserId))))
            {
                throw new UserDoesNotBelongToGroupException(ticket.ProjectId, command.UserId);
            }
        }
        
        ticket.ChangeAssignedUser(command.UserId, clock.Now());
        await ticketRepository.UpdateAsync(ticket);
    }
}