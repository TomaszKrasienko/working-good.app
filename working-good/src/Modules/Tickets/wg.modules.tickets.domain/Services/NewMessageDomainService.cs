using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;

namespace wg.modules.tickets.domain.Services;

internal sealed class NewMessageDomainService(
    ITicketRepository ticketRepository) : INewMessageDomainService
{
//     public Task AddNewMessage(Guid id, string sender, string subject, string content, DateTime createdAt, int? ticketNumber,
//         Guid? ticketId)
//     {
//         if (ticketNumber is null || ticketId is null)
//         {
//             
//         }
//     }
//
//     private async Task<Ticket> AddNewTicket(string subject, string content)
//     {
//         // var maxNumber = await ticketRepository.GetMaxNumberAsync(); 
//         // Ticket.Create(Guid.NewGuid(), maxNumber + 1, subject, content, )
//     }
    public Task AddNewMessage(Guid messageId, string sender, string subject, string content, DateTime createdAt, int? ticketNumber,
        Guid? ticketId)
    {
        throw new NotImplementedException();
    }
}