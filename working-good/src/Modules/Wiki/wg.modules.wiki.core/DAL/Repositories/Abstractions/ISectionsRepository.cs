using wg.modules.wiki.core.Entities;

namespace wg.modules.wiki.core.DAL.Repositories.Abstractions;

public interface ISectionRepository
{
    Task<Section> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsNameExistsAsync(string name, CancellationToken cancellationToken);
    Task AddAsync(Section section, CancellationToken cancellationToken);
}