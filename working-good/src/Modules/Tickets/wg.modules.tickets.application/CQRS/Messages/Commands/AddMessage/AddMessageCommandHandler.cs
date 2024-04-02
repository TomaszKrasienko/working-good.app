using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Events;
using wg.modules.tickets.application.Exceptions;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Messages.Commands.AddMessage;

internal sealed class AddMessageCommandHandler(
    IOwnerApiClient ownerApiClient,
    IClock clock,
    IMessageBroker messageBroker) : ICommandHandler<AddMessageCommand>
{
    public async Task HandleAsync(AddMessageCommand command, CancellationToken cancellationToken)
    {
        
    }
}