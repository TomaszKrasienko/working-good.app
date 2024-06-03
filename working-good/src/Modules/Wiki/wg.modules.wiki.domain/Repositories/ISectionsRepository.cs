using wg.modules.wiki.domain.Entities;

namespace wg.modules.wiki.domain.Repositories;

public interface ISectionRepository
{
    Task<Section> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsNameExistsAsync(string name, CancellationToken cancellationToken);
    Task AddAsync(Section section, CancellationToken cancellationToken);
    Task UpdateAsync(Section section, CancellationToken cancellationToken);
}