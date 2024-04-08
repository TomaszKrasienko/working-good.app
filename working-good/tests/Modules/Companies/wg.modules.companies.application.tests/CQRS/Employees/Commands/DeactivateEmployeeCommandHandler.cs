using NSubstitute;
using wg.modules.companies.application.CQRS.Employees.Commands.DeactivateEmployee;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.Messaging;

namespace wg.modules.companies.application.tests.CQRS.Employees.Commands;

public sealed class DeactivateEmployeeCommandHandlerTests
{
    #region arrange
    private readonly ICompanyRepository _companyRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly DeactivateEmployeeCommandHandler _handler;

    public DeactivateEmployeeCommandHandlerTests()
    {
        _companyRepository = Substitute.For<ICompanyRepository>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new DeactivateEmployeeCommandHandler();
    }

    #endregion
}