using wg.modules.tickets.domain.Entities;

namespace wg.modules.tickets.domain.Services;

internal interface ITicketService
{
    Task<Ticket> Handle(Guid id, int? number, string subject, string content, DateTime createdAt,
        Guid? createdBy, DateTime stateChange, bool isPriority, string state = null, DateTime? expirationDate = null,
        Guid? assignedEmployee = null, Guid? assignedUser = null);
}