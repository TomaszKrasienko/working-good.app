using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.application.CQRS.Companies.AddCompany;

public record AddCompanyCommand(Guid Id, string Name, TimeSpan SlaTime, string EmailDomain) : ICommand;