using wg.modules.companies.application.Events;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Exceptions;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;

namespace wg.modules.companies.application.CQRS.Employees.Commands.DeactivateEmployee;

internal sealed class DeactivateEmployeeCommandHandler(
    ICompanyRepository companyRepository,
    IMessageBroker messageBroker) : ICommandHandler<DeactivateEmployeeCommand>
{
    public async Task HandleAsync(DeactivateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByEmployeeIdAsync(command.Id);
        if (company is null)
        {
            throw new EmployeeNotFoundException(command.Id);
        }

        var substituteEmployee = company.Employees.FirstOrDefault(x => x.Id.Equals(command.SubstitutionEmployeeId));
        if (substituteEmployee is null)
        {
            throw new SubstituteEmployeeIdNotFound(command.SubstitutionEmployeeId);
        }

        if (!substituteEmployee.IsActive)
        {
            throw new SubstituteEmployeeNotActiveException(command.SubstitutionEmployeeId);
        }
        
        company.DeactivateEmployee(command.Id);
        await companyRepository.UpdateAsync(company);
        await messageBroker.PublishAsync(new EmployeeDeactivated(command.Id, command.SubstitutionEmployeeId));
    }
}