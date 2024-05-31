using wg.modules.wiki.core.Services.Commands;

namespace wg.modules.wiki.core.Services.Abstractions;

public interface ISectionService
{
    Task AddAsync(AddSectionCommand command, CancellationToken cancellationToken);
}