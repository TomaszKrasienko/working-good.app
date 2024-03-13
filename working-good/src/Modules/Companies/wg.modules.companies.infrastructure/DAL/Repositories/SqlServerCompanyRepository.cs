using Microsoft.EntityFrameworkCore;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.Repositories;

namespace wg.modules.companies.infrastructure.DAL.Repositories;

internal sealed class SqlServerCompanyRepository(
    CompaniesDbContext context) : ICompanyRepository
{
    private readonly DbSet<Company> _companies = context.Companies;
    
    public async Task AddAsync(Company company)
    {
        await _companies.AddAsync(company);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Company company)
    {
        _companies.Update(company);
        await context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(string name)
        => _companies.AnyAsync(x => x.Name == name);

    public Task<bool> ExistsDomainAsync(string emailDomain)
        => _companies.AnyAsync(x => x.EmailDomain == emailDomain);

    public Task<Company> GetByIdAsync(Guid id)
        => _companies.FirstOrDefaultAsync(x => x.Id.Equals(id));
}