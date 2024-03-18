using wg.modules.tickets.domain.Entities;

namespace wg.modules.tickets.domain.Repositories;

public interface ITicketRepository
{
    Task<Ticket> GetByNumberAsync(int number);
    Task<Ticket> GetByIdAsync(Guid id);
    Task<int> GetMaxNumberAsync();
    Task AddAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
}