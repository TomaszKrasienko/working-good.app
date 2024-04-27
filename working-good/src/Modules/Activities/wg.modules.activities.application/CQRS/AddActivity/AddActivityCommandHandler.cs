using wg.modules.activities.application.Clients;
using wg.modules.activities.application.Clients.DTOs;
using wg.modules.activities.application.Exceptions;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.activities.application.CQRS.AddActivity;

internal sealed class AddActivityCommandHandler(
    IDailyEmployeeActivityRepository dailyEmployeeActivityRepository,
    ITicketsApiClient ticketsApiClient) : ICommandHandler<AddActivityCommand>
{
    public async Task HandleAsync(AddActivityCommand command, CancellationToken cancellationToken)
    {
        var isTicketAvailable = await ticketsApiClient
            .IsAvailableForChangesTicketExists(new TicketIdDto(command.TicketId));
        
        if (!isTicketAvailable.Value)
        {
            throw new TicketWithStateForChangesNotFoundException(command.TicketId);
        }

        var dailyEmployeeActivity = await dailyEmployeeActivityRepository
            .GetByDateForUser(command.TimeFrom.Date, command.UserId);

        if (dailyEmployeeActivity is null)
        {
            dailyEmployeeActivity = DailyEmployeeActivity.Create(Guid.NewGuid(),
                command.TimeFrom.Date, command.UserId);

            if (command.IsPaid)
            {
                dailyEmployeeActivity.AddPaidActivity(command.Id, command.Content,
                    command.TicketId, command.TimeFrom, command.TimeTo);
            }
            else
            {
                dailyEmployeeActivity.AddInternalActivity(command.Id, command.Content,
                    command.TicketId, command.TimeFrom, command.TimeTo);
            }

            await dailyEmployeeActivityRepository.AddAsync(dailyEmployeeActivity);
        }
        if (command.IsPaid)
        {
            dailyEmployeeActivity.AddPaidActivity(command.Id, command.Content,
                command.TicketId, command.TimeFrom, command.TimeTo);
        }
        else
        {
            dailyEmployeeActivity.AddInternalActivity(command.Id, command.Content,
                command.TicketId, command.TimeFrom, command.TimeTo);
        }

        await dailyEmployeeActivityRepository.UpdateAsync(dailyEmployeeActivity);
    }
}