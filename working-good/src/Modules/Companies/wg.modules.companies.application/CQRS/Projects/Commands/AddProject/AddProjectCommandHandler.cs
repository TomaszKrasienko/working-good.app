using wg.modules.companies.application.Events;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;

namespace wg.modules.companies.application.CQRS.Projects.Commands.AddProject;

internal sealed class AddProjectCommandHandler(
    ICompanyRepository companyRepository,
    IMessageBroker messageBroker) : ICommandHandler<AddProjectCommand>
{
    
    public async Task HandleAsync(AddProjectCommand command, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByIdAsync(command.CompanyId);
        if (company is null)
        {
            throw new CompanyNotFoundException(command.CompanyId);
        }
        
        company.AddProject(command.Id, command.Title, command.Description, 
            command.PlannedStart, command.PlannedFinish);
        await companyRepository.UpdateAsync(company);
        await messageBroker.PublishAsync(new ProjectAdded(command.Id, command.Title));
    }
}