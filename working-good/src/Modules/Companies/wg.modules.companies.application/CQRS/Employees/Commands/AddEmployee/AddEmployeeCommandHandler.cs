using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.application.CQRS.Employees.Commands.AddEmployee;

internal sealed class AddEmployeeCommandHandler(
    ICompanyRepository companyRepository) : ICommandHandler<AddEmployeeCommand>
{
    public async Task HandleAsync(AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByIdAsync(command.CompanyId);

        if (company is null)
        {
            throw new CompanyNotFoundException(command.CompanyId);
        }
        
        company.AddEmployee(command.Id, command.Email, command.PhoneNumber);
        await companyRepository.UpdateAsync(company);
    }
}