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
        => _companies.AnyAsync(x => !string.IsNullOrWhiteSpace(name) && x.Name == name);

    public Task<bool> ExistsDomainAsync(string emailDomain)
        => _companies.AnyAsync(x => x.EmailDomain == emailDomain);

    public Task<Company> GetByIdAsync(Guid id)
        => _companies
            .Include(x => x.Projects)
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

    public Task<Company> GetByProjectIdAsync(Guid projectId)
        => _companies
            .Include(x => x.Projects)
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(x => x.Projects.Any(p => p.Id.Equals(projectId)));

    public Task<Company> GetByEmployeeIdAsync(Guid employeeId)
        => _companies
            .Include(x => x.Projects)
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(x => x.Employees.Any(e => e.Id.Equals(employeeId)));
}