using Microsoft.EntityFrameworkCore;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.Repositories;

namespace wg.modules.owner.infrastructure.DAL.Repositories;

internal sealed class SqlServerOwnerRepository : IOwnerRepository
{
    private OwnerDbContext _context;
    private DbSet<Owner> _owner;
    
    public SqlServerOwnerRepository(OwnerDbContext context)
    {
        _context = context;
        _owner = context.Owner;
    }
    
    public async Task AddAsync(Owner owner)
    {
        await _owner.AddAsync(owner);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Owner owner)
    {
        _owner.Update(owner);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync()
        => _owner.AnyAsync();

    public Task<Owner> GetAsync()
        => _owner
            .Include(x => x.Users)
            .FirstOrDefaultAsync();
}