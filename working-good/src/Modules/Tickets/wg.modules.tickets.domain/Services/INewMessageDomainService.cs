using System;
using System.Threading.Tasks;
using wg.modules.tickets.domain.Entities;

namespace wg.modules.tickets.domain.Services;

public interface INewMessageDomainService
{
    Task<Ticket> AddNewMessage(Guid id, string sender, string subject, string content,
        DateTime createdAt, int? ticketNumber, Guid? ticketId, Guid? employeeId);
}