using System;
using System.Threading.Tasks;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Exceptions;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.ValueObjects.Ticket;

namespace wg.modules.tickets.domain.Services;

internal sealed class NewMessageDomainService(
    ITicketRepository ticketRepository) : INewMessageDomainService
{
    public async Task<Ticket> AddNewMessage(Guid id, string sender, string subject, string content, DateTime createdAt,
        int? ticketNumber, Guid? ticketId, Guid? employeeId)
    {
        if (ticketNumber is null && ticketId is null)
        {
            var maxNumber = await ticketRepository.GetMaxNumberAsync();
            var newTicket = Ticket.Create(id, maxNumber + 1, subject, content,
                createdAt, employeeId ?? Guid.Empty, State.New(), createdAt, false, null,
                employeeId);
            await ticketRepository.AddAsync(newTicket);
            return newTicket;
        }

        Ticket ticket = null;
        if (ticketId is not null)
        {
            ticket = await ticketRepository.GetByIdAsync((Guid)ticketId);
            if (ticket is null)
            {
                throw new TicketNotFoundException((Guid)ticketId);
            }
        }

        if (ticketNumber is not null && ticket is null)
        {
            ticket = await ticketRepository.GetByNumberAsync((int)ticketNumber);
            if (ticket is null)
            {
                throw new TicketNotFoundException((int)ticketNumber);
            }
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            subject = ticket.Subject;
        }
        
        ticket.AddMessage(id, sender, subject, content, createdAt);
        await ticketRepository.UpdateAsync(ticket);
        return ticket;
    }
}