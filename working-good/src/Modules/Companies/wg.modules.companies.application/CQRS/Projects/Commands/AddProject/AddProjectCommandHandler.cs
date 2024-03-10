using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;

namespace wg.modules.companies.application.CQRS.Projects.Commands.AddProject;

internal sealed class AddProjectCommandHandler(
    ICompanyRepository companyRepository
    ) : ICommandHandler<AddProjectCommand>
{
    
    public Task HandleAsync(AddProjectCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}