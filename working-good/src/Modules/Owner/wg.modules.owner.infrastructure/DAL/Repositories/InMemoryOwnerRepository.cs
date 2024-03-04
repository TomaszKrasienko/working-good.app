using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.Repositories;

namespace wg.modules.owner.infrastructure.DAL.Repositories;

internal sealed class InMemoryOwnerRepository : IOwnerRepository
{
    private readonly List<Owner> _owners = new List<Owner>();

    public Task AddAsync(Owner owner)
    {
        _owners.Add(owner);
        return Task.CompletedTask;
    } 

    public Task UpdateAsync(Owner owner)
    {
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync()
        => Task.FromResult(_owners.Any());

    public Task<Owner> GetAsync()
        => Task.FromResult(_owners.FirstOrDefault());
}