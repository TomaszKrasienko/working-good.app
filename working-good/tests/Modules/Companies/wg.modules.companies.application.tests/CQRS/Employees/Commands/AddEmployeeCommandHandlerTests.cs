using NSubstitute;
using Shouldly;
using wg.modules.companies.application.CQRS.Employees.Commands.AddEmployee;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Entities;
using wg.modules.companies.domain.Repositories;
using wg.tests.shared.Factories.Companies;
using Xunit;

namespace wg.modules.companies.application.tests.CQRS.Employees.Commands;

public sealed class AddEmployeeCommandHandlerTests
{
    private Task Act(AddEmployeeCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenExistingCompanyIdAndAddEmployeeCommand_ShouldUpdateCompanyByRepository()
    {
        //arrange
        var company = CompanyFactory.Get();
        var command = new AddEmployeeCommand(company.Id, Guid.NewGuid(),
            $"test@{company.EmailDomain.Value}", "500 500 500");
        _companyRepository
            .GetByIdAsync(command.CompanyId)
            .Returns(company);
        
        //act
        await Act(command);
        
        //assert
        await _companyRepository
            .Received(1)
            .UpdateAsync(company);
    }

    [Fact]
    public async Task? HandleAsync_GivenNotExistingCompanyId_ShouldThrowCompanyNotFoundException()
    {
        //arrange
        var command = new AddEmployeeCommand(Guid.NewGuid(), Guid.NewGuid(), "test@test.pl", "500 500 500");
        await _companyRepository
            .GetByIdAsync(command.CompanyId);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<CompanyNotFoundException>();
    }
    
    #region arrange
    private readonly ICompanyRepository _companyRepository;
    private readonly AddEmployeeCommandHandler _handler;
    
    public AddEmployeeCommandHandlerTests()
    {
        _companyRepository = Substitute.For<ICompanyRepository>();
        _handler = new AddEmployeeCommandHandler(_companyRepository);
    }
    #endregion
}