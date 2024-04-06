using wg.modules.companies.application.Events;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Exceptions;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;

namespace wg.modules.companies.application.CQRS.Projects.Commands.EditProject;

internal sealed class EditProjectCommandHandler(
    ICompanyRepository companyRepository,
    IMessageBroker messageBroker) : ICommandHandler<EditProjectCommand>
{
    public async Task HandleAsync(EditProjectCommand command, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByProjectIdAsync(command.Id);
        if (company is null)
        {
            throw new ProjectNotFoundException(command.Id);
        }
        company.EditProject(command.Id, command.Title, command.Description,
            command.PlannedStart, command.PlannedFinish);
        
        await companyRepository.UpdateAsync(company);
        await messageBroker.PublishAsync(new ProjectEdited(command.Id, command.Title));
    }
}