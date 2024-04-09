using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;

namespace wg.modules.tickets.infrastructure.DAL.Repositories;

internal sealed class SqlServerTicketRepository : ITicketRepository
{
    private readonly TicketsDbContext _dbContext;
    private readonly DbSet<Ticket> _tickets;

    public SqlServerTicketRepository(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
        _tickets = _dbContext.Tickets;
    }

    public Task<List<Ticket>> GetAllForAssignedEmployee(Guid employeeId)
        => _tickets
            .Include(x => x.Messages)
            .Where(x => x.AssignedEmployee.Equals(employeeId))
            .ToListAsync();

    public Task<Ticket> GetByNumberAsync(int number)
        => _tickets
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(x => x.Number == number);

    public Task<Ticket> GetByIdAsync(Guid id)
        => _tickets
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

    public async Task<int> GetMaxNumberAsync()
    {
        var tickets = await _dbContext
            .Tickets
            .AsNoTracking()
            .ToListAsync();
        return tickets.Max(x => (int?)x.Number.Value) ?? 0;
    }

    public async Task AddAsync(Ticket ticket)
    {
        await _tickets.AddAsync(ticket);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        _tickets.Update(ticket);
        await _dbContext.SaveChangesAsync();
    }
}