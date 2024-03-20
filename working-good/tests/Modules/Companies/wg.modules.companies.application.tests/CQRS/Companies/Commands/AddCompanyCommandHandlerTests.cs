using NSubstitute;
using Shouldly;
using wg.modules.companies.application.CQRS.Companies.Commands.AddCompany;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.Repositories;
using Xunit;

namespace wg.modules.companies.application.tests.CQRS.Companies.Commands;

public sealed class AddCompanyCommandHandlerTests
{
    private Task Act(AddCompanyCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task Handle_GivenAddCompanyCommand_ShouldAddCompanyByRepository()
    {
        //arrange
        var command = new AddCompanyCommand(Guid.NewGuid(), "MyCompany", TimeSpan.FromHours(6), "test.pl");
        _companyRepository
            .ExistsAsync(command.Name)
            .Returns(false);
        _companyRepository
            .ExistsDomainAsync(command.EmailDomain)
            .Returns(false);
        
        //act
        await Act(command);
        
        //assert
        await _companyRepository
            .Received(1)
            .AddAsync(Arg.Is<Company>(arg
                => arg.Id.Value == command.Id
                   && arg.EmailDomain == command.EmailDomain
                   && arg.Name == command.Name
                   && arg.SlaTime.Value == command.SlaTime));
    }

    [Fact]
    public async Task Handle_GivenExistingCompanyName_ShouldThrowCompanyNameAlreadyInUseException()
    {
        //arrange
        var command = new AddCompanyCommand(Guid.NewGuid(), "MyCompany", TimeSpan.FromHours(6), "test.pl");
        _companyRepository
            .ExistsAsync(command.Name)
            .Returns(true);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<CompanyNameAlreadyInUseException>();
    }

    [Fact]
    public async Task Handle_GivenExistingEmailDomain_ShouldThrowEmailDomainAlreadyInUseException()
    {        
        //arrange
        var command = new AddCompanyCommand(Guid.NewGuid(), "MyCompany", TimeSpan.FromHours(6), "test.pl");
        _companyRepository
            .ExistsAsync(command.Name)
            .Returns(false);
        _companyRepository
            .ExistsDomainAsync(command.EmailDomain)
            .Returns(true);
                
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<EmailDomainAlreadyInUseException>();
    }

    #region arrange
    private readonly ICompanyRepository _companyRepository;
    private readonly AddCompanyCommandHandler _handler;
    public AddCompanyCommandHandlerTests()
    {
        _companyRepository = Substitute.For<ICompanyRepository>();
        _handler = new AddCompanyCommandHandler(_companyRepository);
    }
    #endregion
}