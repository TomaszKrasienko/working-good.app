using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AssignUser;

internal sealed class AssignUserCommandHandler(
    ITicketRepository ticketRepository,
    IOwnerApiClient ownerApiClient,
    IMessageBroker messageBroker) : ICommandHandler<AssignUserCommand>
{
    public async Task HandleAsync(AssignUserCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.TicketId);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.TicketId);
        }

        if (ticket.ProjectId is not null)
        {
            var isMembershipExists = await ownerApiClient.IsMembershipExistsAsync(new GetMembershipDto()
            {
                GroupId = ticket.ProjectId,
                UserId = command.UserId
            });
            if (!isMembershipExists.Value)
            {
                throw new UserDoesNotBelongToGroupException(ticket.ProjectId, command.UserId);
            }
        }
        
        ticket.ChangeAssignedUser(command.UserId);
        await ticketRepository.UpdateAsync(ticket);

        var @event = new UserAssigned(ticket.Id, ticket.Number, ticket.AssignedUser);
        await messageBroker.PublishAsync(@event);
    }
}