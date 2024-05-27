using wg.modules.notifications.core.Clients.Companies;
using wg.modules.notifications.core.Clients.Companies.DTO;
using wg.modules.notifications.core.Providers.Abstractions;
using wg.modules.notifications.core.Services.Abstractions;
using wg.shared.abstractions.Events;

namespace wg.modules.notifications.core.Events.External.Handlers;

internal sealed class EmployeeAddedHandler(
    ICompaniesApiClient companiesApiClient,
    IEmailNotificationProvider emailNotificationProvider,
    IEmailPublisher emailPublisher) : IEventHandler<EmployeeAdded>
{
    public async Task HandleAsync(EmployeeAdded @event)
    {
        var employeeDto = await companiesApiClient.GetEmployeeByIdAsync(new EmployeeIdDto()
        {
            Id = @event.Id
        });

        var emailNotification = emailNotificationProvider.GetForNewEmployee(employeeDto.Email);
        await emailPublisher.PublishAsync(emailNotification, default);
    }
}