using wg.modules.activities.application.Clients;
using wg.modules.activities.application.Clients.Tickets;
using wg.modules.activities.application.Clients.Tickets.DTOs;
using wg.modules.activities.application.Exceptions;
using wg.modules.activities.domain.Entities;
using wg.modules.activities.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.activities.application.CQRS.AddActivity;

internal sealed class AddActivityCommandHandler(
    IDailyUserActivityRepository dailyUserActivityRepository,
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

        var dailyUserActivity = await dailyUserActivityRepository
            .GetByDateForUser(command.TimeFrom.Date, command.UserId);

        if (dailyUserActivity is null)
        {
            dailyUserActivity = DailyUserActivity.Create(command.TimeFrom.Date, command.UserId);

            if (command.IsPaid)
            {
                dailyUserActivity.AddPaidActivity(command.Id, command.Content,
                    command.TicketId, command.TimeFrom, command.TimeTo);
            }
            else
            {
                dailyUserActivity.AddInternalActivity(command.Id, command.Content,
                    command.TicketId, command.TimeFrom, command.TimeTo);
            }

            await dailyUserActivityRepository.AddAsync(dailyUserActivity);
        }
        if (command.IsPaid)
        {
            dailyUserActivity.AddPaidActivity(command.Id, command.Content,
                command.TicketId, command.TimeFrom, command.TimeTo);
        }
        else
        {
            dailyUserActivity.AddInternalActivity(command.Id, command.Content,
                command.TicketId, command.TimeFrom, command.TimeTo);
        }

        await dailyUserActivityRepository.UpdateAsync(dailyUserActivity);
    }
}