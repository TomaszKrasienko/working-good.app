using wg.modules.owner.domain.Entities;

namespace wg.modules.owner.domain.Repositories;

public interface IOwnerRepository
{
    Task AddAsync(Owner owner);
    Task UpdateAsync(Owner owner);
    Task<bool> ExistsAsync();
    Task<Owner> GetAsync(); 
}