using Microsoft.EntityFrameworkCore;
using wg.modules.wiki.core.DAL.Repositories.Abstractions;
using wg.modules.wiki.core.Entities;

namespace wg.modules.wiki.core.DAL.Repositories.Internals;

internal sealed class SqlServerSectionRepository(
    WikiDbContext dbContext) : ISectionRepository
{
    public Task<Section> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => dbContext
            .Sections
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

    public Task<bool> IsNameExistsAsync(string name, CancellationToken cancellationToken)
        => dbContext
            .Sections
            .AsNoTracking()
            .AnyAsync(x => x.Name == name, cancellationToken);

    public async Task AddAsync(Section section, CancellationToken cancellationToken)
    {
        await dbContext.Sections.AddAsync(section, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Section section, CancellationToken cancellationToken)
    {
        dbContext.Sections.Update(section);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}