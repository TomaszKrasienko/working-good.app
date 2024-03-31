using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Services;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Messages.Commands.AddMessage;

internal sealed class AddMessageCommandHandler(
    IOwnerApiClient ownerApiClient,
    IClock clock,
    INewMessageDomainService newMessageDomainService,
    IMessageBroker messageBroker) : ICommandHandler<AddMessageCommand>
{
    public async Task HandleAsync(AddMessageCommand command, CancellationToken cancellationToken)
    {
        var user = await ownerApiClient.GetUserByIdAsyncAsync(new UserIdDto(command.UserId));
        if (user is null)
        {
            throw new UserNotFoundException(command.UserId);
        }

        var ticket = await newMessageDomainService.AddNewMessage(command.Id, user.Email, null, command.Content,
            clock.Now(), null, command.TicketId, null);
        var recipients = ticket
            .Messages
            .Select(x => x.Sender.Value)
            .ToArray();
        var @event = new MessageAdded(ticket.Number, ticket.Subject, command.Content, recipients);
        await messageBroker.PublishAsync(@event);
    }
}