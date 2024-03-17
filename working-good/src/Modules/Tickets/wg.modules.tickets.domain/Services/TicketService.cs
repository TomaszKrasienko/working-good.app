using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;

namespace wg.modules.tickets.domain.Services;

internal sealed class TicketService(
    ITicketRepository ticketRepository) : ITicketService
{
    public async Task<Ticket> Handle(Guid id, int? number, string subject, string content, DateTime createdAt,
        Guid? createdBy, DateTime stateChange, bool isPriority, string state = null, DateTime? expirationDate = null,
        Guid? assignedEmployee = null, Guid? assignedUser = null)
    {
        if (number is null)
        {
            int maxNumber = await ticketRepository.GetMaxNumberAsync();
            return Ticket.Create(id, maxNumber + 1, subject, content,
                createdAt, createdBy, State.New(), stateChange, isPriority, expirationDate, assignedEmployee,
                assignedUser);

        }

        var ticket = await ticketRepository.GetByNumberAsync((int)number);
        if (ticket is null)
        {
            throw new NumberDoesNotExistsException((int)number);
        }

        
        return ticket;
    }
}