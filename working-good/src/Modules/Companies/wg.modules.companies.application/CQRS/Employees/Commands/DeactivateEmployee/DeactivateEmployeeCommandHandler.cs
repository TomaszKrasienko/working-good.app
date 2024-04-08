using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.application.CQRS.Employees.Commands.DeactivateEmployee;

internal sealed class DeactivateEmployeeCommandHandler : ICommandHandler<DeactivateEmployeeCommand>
{
    public Task HandleAsync(DeactivateEmployeeCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}