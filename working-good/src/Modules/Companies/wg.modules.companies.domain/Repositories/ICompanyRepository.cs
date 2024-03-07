using wg.modules.companies.domain.Entities;

namespace wg.modules.companies.domain.Repositories;

public interface ICompanyRepository
{
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
    Task<bool> ExistsAsync(string name);
    Task<bool> ExistsDomainAsync(string emailDomain);
    Task<Company> GetByIdAsync(Guid id);
}