using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.application.CQRS.Companies.Commands.UpdateCompany;

internal sealed class UpdateCompanyCommandHandler(
    ICompanyRepository companyRepository) : ICommandHandler<UpdateCompanyCommand>
{
    public Task HandleAsync(UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}