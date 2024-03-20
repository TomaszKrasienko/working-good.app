using NSubstitute;
using Shouldly;
using wg.modules.companies.application.CQRS.Projects.Commands.AddProject;
using wg.modules.companies.application.Events;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.Messaging;
using wg.sharedForTests.Factories.Companies;
using Xunit;

namespace wg.modules.companies.application.tests.CQRS.Projects.Commands;

public sealed class AddProjectCommandHandlerTests
{
    private Task Act(AddProjectCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingCompanyIdAndValidArguments_ShouldUpdateCompanyWithProjectAndSendEvent()
    {
        //arrange
        var company = CompanyFactory.Get();
        var command = new AddProjectCommand(company.Id, Guid.NewGuid(), "My project title",
            "Description of my project", DateTime.Now, DateTime.Now.AddMonths(4));
        _companyRepository
            .GetByIdAsync(company.Id)
            .Returns(company);
        
        //act
        await Act(command);
        
        //assert
        await _companyRepository
            .Received(1)
            .UpdateAsync(company);
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<ProjectAdded>(arg
                => arg.Id == command.Id
                   && arg.Title == command.Title));
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingCompanyId_ShouldThrowCompanyNotFoundException()
    {
        //arrange
        var command = new AddProjectCommand(Guid.NewGuid(), Guid.NewGuid(), "My project title",
            "Description of my project", DateTime.Now, DateTime.Now.AddMonths(4));
        
        //act
        var exception = await Record.ExceptionAsync(async() => await Act(command));
        
        //assert
        exception.ShouldBeOfType<CompanyNotFoundException>();
        await _companyRepository.Received(0).UpdateAsync(Arg.Any<Company>());
        await _messageBroker.Received(0).PublishAsync(Arg.Any<ProjectAdded>());
    }
    
    #region arrange
    private readonly ICompanyRepository _companyRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly AddProjectCommandHandler _handler;

    public AddProjectCommandHandlerTests()
    {
        _companyRepository = Substitute.For<ICompanyRepository>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new AddProjectCommandHandler(_companyRepository, _messageBroker);
    }
    #endregion
}