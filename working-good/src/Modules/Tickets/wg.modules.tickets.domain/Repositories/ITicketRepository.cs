using wg.modules.tickets.domain.Entities;

namespace wg.modules.tickets.domain.Repositories;

public interface ITicketRepository
{
    Task<Ticket> GetByNumberAsync(int number);
    Task<int> GetMaxNumberAsync();
    Task AddAsync(Ticket ticket);
}