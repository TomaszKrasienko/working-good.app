using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Messages.Commands.AddMessage;

internal sealed class AddMessageCommandHandler(
    ITicketRepository ticketRepository,
    IOwnerApiClient ownerApiClient,
    IMessageBroker messageBroker,
    IClock clock) : ICommandHandler<AddMessageCommand>
{
    public async Task HandleAsync(AddMessageCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(command.TicketId);
        if (ticket is null)
        {
            throw new TicketNotFoundException(command.TicketId);
        }

        var user = await ownerApiClient.GetActiveUserByIdAsync(new UserIdDto(command.UserId));
        if (user is null)
        {
            throw new UserNotFoundException(command.UserId);
        }
        
        ticket.AddMessage(command.Id, user.Email, ticket.Subject, command.Content, 
            clock.Now());
        await ticketRepository.UpdateAsync(ticket);
        var messageSent = new MessageAdded(ticket.Number, ticket.Subject, command.Content,
            ticket.AssignedEmployee);
        await messageBroker.PublishAsync(messageSent);
    }
}