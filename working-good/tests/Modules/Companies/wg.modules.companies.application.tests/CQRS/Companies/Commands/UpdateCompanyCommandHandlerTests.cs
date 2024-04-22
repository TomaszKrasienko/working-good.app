using NSubstitute;
using Shouldly;
using wg.modules.companies.application.CQRS.Companies.Commands.UpdateCompany;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Companies;
using Xunit;

namespace wg.modules.companies.application.tests.CQRS.Companies.Commands;

public sealed class UpdateCompanyCommandHandlerTests
{
    private Task Act(UpdateCompanyCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenExistingCompanyId_ShouldUpdateCompanyByRepository()
    {
        //arrange
        var company = CompanyFactory.Get(1).Single();
        var command = new UpdateCompanyCommand(company.Id, "New name", company.SlaTime.Value.Add(TimeSpan.FromHours(1)));
        
        _companyRepository
            .GetByIdAsync(command.Id)
            .Returns(company);
        
        //act
        await Act(command);
        
        //assert
        await _companyRepository
            .Received(1)
            .UpdateAsync(company);
        
        company.Name.Value.ShouldBe(command.Name);
        company.SlaTime.Value.ShouldBe(command.SlaTime);
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingCompanyId_ShouldThrowCompanyNotFoundException()
    {
        //arrange
        var command = new UpdateCompanyCommand(Guid.NewGuid(), "New name", TimeSpan.FromHours(1));
        
        //act
        var exception = await Record.ExceptionAsync(async() => await Act(command));
        
        //assert
        exception.ShouldBeOfType<CompanyNotFoundException>();
    }

    private readonly ICompanyRepository _companyRepository;
    private readonly ICommandHandler<UpdateCompanyCommand> _handler;
    
    public UpdateCompanyCommandHandlerTests()
    {
        _companyRepository = Substitute.For<ICompanyRepository>();
        _handler = new UpdateCompanyCommandHandler(_companyRepository);
    }
}