using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.application.CQRS.Companies.Commands.UpdateCompany;

internal sealed class UpdateCompanyCommandHandler(
    ICompanyRepository companyRepository) : ICommandHandler<UpdateCompanyCommand>
{
    public async Task HandleAsync(UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByIdAsync(command.Id);
        if (company is null)
        {
            throw new CompanyNotFoundException(command.Id);
        }
        
        company.ChangeName(command.Name);
        company.ChangeSlaTime(command.SlaTime);
        await companyRepository.UpdateAsync(company);
    }
}