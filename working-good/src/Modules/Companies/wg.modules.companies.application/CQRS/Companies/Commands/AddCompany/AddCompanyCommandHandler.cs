using wg.modules.companies.application.Events;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;

namespace wg.modules.companies.application.CQRS.Companies.Commands.AddCompany;

internal sealed class AddCompanyCommandHandler(
    ICompanyRepository companyRepository,
    IMessageBroker messageBroker) : ICommandHandler<AddCompanyCommand>
{
    public async Task HandleAsync(AddCompanyCommand command, CancellationToken cancellationToken)
    {
        if (await companyRepository.ExistsAsync(command.Name))
        {
            throw new CompanyNameAlreadyInUseException(command.Name);
        }

        if (await companyRepository.ExistsDomainAsync(command.EmailDomain))
        {
            throw new EmailDomainAlreadyInUseException(command.EmailDomain);
        }

        var company = Company.Create(command.Id, command.Name, command.SlaTime, command.EmailDomain);
        await companyRepository.AddAsync(company);
        var @event = new CompanyAdded(company.Id, company.Name);
        await messageBroker.PublishAsync(@event);
    }
}