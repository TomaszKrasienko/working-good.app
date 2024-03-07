using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.Repositories;

namespace wg.modules.companies.infrastructure.DAL.Repositories;

internal sealed class InMemoryCompanyRepository : ICompanyRepository
{
    private readonly List<Company> _companies = new List<Company>();
    public Task AddAsync(Company company)
    {
        _companies.Add(company);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Company company)
    {
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string name)
        => Task.FromResult(_companies.Any(x => x.Name == name));

    public Task<bool> ExistsDomainAsync(string emailDomain)
        => Task.FromResult(_companies.Any(x => x.EmailDomain == emailDomain));

    public Task<Company> GetByIdAsync(Guid id)
        => Task.FromResult(_companies.FirstOrDefault(x => x.Id.Equals(id)));
}