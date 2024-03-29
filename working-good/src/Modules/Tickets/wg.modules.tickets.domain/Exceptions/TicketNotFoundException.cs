using System;
using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class TicketNotFoundException : WgException
{
    public TicketNotFoundException(int ticketNumber)
        : base($"Ticket with number: {ticketNumber} not found")
    {
        
    }
    
    public TicketNotFoundException(Guid ticketId)
        : base($"Ticket with ID: {ticketId} not found")
    {
        
    }
}
    