using NSubstitute;
using wg.modules.companies.application.CQRS.Projects.Commands.EditProject;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;

namespace wg.modules.companies.application.tests.CQRS.Projects.Commands;

public sealed class EditProjectCommandHandlerTests
{
    private Task Act(EditProjectCommand command) => _handler.HandleAsync(command, default);
    
    #region arrange

    private readonly ICompanyRepository _companyRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly ICommandHandler<EditProjectCommand> _handler;

    public EditProjectCommandHandlerTests()
    {
        _companyRepository = Substitute.For<ICompanyRepository>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new EditProjectCommandHandler(_companyRepository, _messageBroker);
    }
    
    #endregion
}