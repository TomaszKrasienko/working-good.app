using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.application.CQRS.Companies.Commands.UpdateCompany;

public sealed record UpdateCompanyCommand(Guid Id, string Name, TimeSpan SlaTime) : ICommand;