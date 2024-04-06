using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;

namespace wg.modules.companies.application.CQRS.Projects.Commands.EditProject;

internal sealed class EditProjectCommandHandler(
    ICompanyRepository companyRepository,
    IMessageBroker messageBroker) : ICommandHandler<EditProjectCommand>
{
    public Task HandleAsync(EditProjectCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}