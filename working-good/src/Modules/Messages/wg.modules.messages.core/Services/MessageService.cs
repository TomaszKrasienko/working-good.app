using wg.modules.messages.core.Clients.Companies;
using wg.modules.messages.core.Events;
using wg.modules.messages.core.Services.Abstractions;
using wg.modules.messages.core.Services.Commands;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;

namespace wg.modules.messages.core.Services;

internal sealed class MessageService(
    ICompaniesApiClient companiesApiClient,
    IClock clock,
    IMessageBroker messageBroker) : IMessageService
{
    public async Task CreateMessage(CreateMessage command)
    {
        var @event = new MessageReceived(command.Email, command.Subject, command.Content,
            clock.Now(), Guid.Empty, command.TicketNumber);
        await messageBroker.PublishAsync(@event);
    }
}