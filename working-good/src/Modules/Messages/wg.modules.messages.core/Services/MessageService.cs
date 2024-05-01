using wg.modules.messages.core.Clients.Companies;
using wg.modules.messages.core.Clients.Companies.DTO;
using wg.modules.messages.core.Events;
using wg.modules.messages.core.Exceptions;
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
        var employee = await companiesApiClient.GetEmployeeByEmailAsync(new EmployeeEmailDto(command.Email));
        if (employee is null)
        {
            throw new EmployeeNotFoundException(command.Email);
        }
        var @event = new MessageReceived(command.Email, command.Subject, command.Content,
            clock.Now(), employee.Id, command.TicketNumber);
        await messageBroker.PublishAsync(@event);
    }
}