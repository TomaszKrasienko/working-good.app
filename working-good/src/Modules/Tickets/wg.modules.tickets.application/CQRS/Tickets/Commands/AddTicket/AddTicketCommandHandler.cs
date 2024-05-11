using wg.modules.tickets.application.Clients.Companies;
using wg.modules.tickets.application.Clients.Companies.DTO;
using wg.modules.tickets.application.Clients.Owner;
using wg.modules.tickets.application.Clients.Owner.DTO;
using wg.modules.tickets.application.Events.Mappers;
using wg.modules.tickets.application.Exceptions;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;
using wg.shared.abstractions.Time;

namespace wg.modules.tickets.application.CQRS.Tickets.Commands.AddTicket;

internal sealed class AddTicketCommandHandler(
    ITicketRepository ticketRepository,
    IOwnerApiClient ownerApiClient,
    IClock clock) : ICommandHandler<AddTicketCommand>
{
    public async Task HandleAsync(AddTicketCommand command, CancellationToken cancellationToken)
    {
        var existingTicket = await ticketRepository.GetByIdAsync(command.Id);
        if (existingTicket is not null)
        {
            throw new TicketAlreadyRegisteredException(command.Id);
        }
        
        var userIdDto = new UserIdDto(command.CreatedBy);
        var userDto = await ownerApiClient.GetActiveUserByIdAsync(userIdDto);

        if (userDto is null)
        {
            throw new AuthorUserNotFoundException(command.CreatedBy);
        }
        
        var maxNumber = await ticketRepository.GetMaxNumberAsync();
        var ticket = Ticket.Create(command.Id, maxNumber + 1, command.Subject, command.Content,
            clock.Now(), userDto.Email);

        await ticketRepository.AddAsync(ticket);
    }
}