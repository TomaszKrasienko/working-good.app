using NSubstitute;
using Shouldly;
using wg.modules.companies.application.CQRS.Projects.Commands.EditProject;
using wg.modules.companies.application.Events;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Exceptions;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.shared.abstractions.Messaging;
using wg.tests.shared.Factories.Companies;
using Xunit;

namespace wg.modules.companies.application.tests.CQRS.Projects.Commands;

public sealed class EditProjectCommandHandlerTests
{
    private Task Act(EditProjectCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingProjectId_ShouldUpdateProjectByRepository()
    {
        //arrange
        var company = CompanyFactory.Get().Single();;
        var project = ProjectFactory.GetInCompany(company, true, true);
        _companyRepository
            .GetByProjectIdAsync(project.Id)
            .Returns(company);
        
        var command = new EditProjectCommand(project.Id, "ProjectNewTitle", "ProjectNewDescription",
            DateTime.Now.AddDays(1), DateTime.Now.AddDays(20));
        
        //act
        await Act(command);
        
        //assert
        await _companyRepository
            .Received(1)
            .UpdateAsync(company);

        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<ProjectEdited>(arg
                => arg.Id == command.Id
                   && arg.Title == command.Title));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingProject_ShouldThrowProjectNotFoundException()
    {
        //arrange
        var command = new EditProjectCommand(Guid.NewGuid(), "Title", "Description");
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));

        //assert
        exception.ShouldBeOfType<ProjectNotFoundException>();
    }
    
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