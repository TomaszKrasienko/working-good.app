using NSubstitute;
using Shouldly;
using wg.modules.companies.application.CQRS.Employees.Commands.DeactivateEmployee;
using wg.modules.companies.application.Events;
using wg.modules.companies.application.Exceptions;
using wg.modules.companies.domain.Exceptions;
using wg.modules.companies.domain.Repositories;
using wg.shared.abstractions.Messaging;
using wg.tests.shared.Factories.Companies;
using Xunit;

namespace wg.modules.companies.application.tests.CQRS.Employees.Commands;

public sealed class DeactivateEmployeeCommandHandlerTests
{
    private Task Act(DeactivateEmployeeCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenExistingIdAndSubstitutionEmployeeId_ShouldUpdateEmployeeAndSendEvent()
    {
        //arrange
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetEmployeeInCompany(company);
        var substituteEmployee = EmployeeFactory.GetEmployeeInCompany(company);

        _companyRepository
            .GetByEmployeeIdAsync(employee.Id)
            .Returns(company);
        var command = new DeactivateEmployeeCommand(employee.Id, substituteEmployee.Id);
        
        //act
        await Act(command);

        //arrange
        var updatedEmployee = company.Employees.FirstOrDefault(x => x.Id == employee.Id);
        updatedEmployee.ShouldNotBeNull();
        updatedEmployee.IsActive.Value.ShouldBeFalse();
        
        await _companyRepository
            .Received(1)
            .UpdateAsync(company);

        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<EmployeeDeactivated>(arg
                => arg.EmployeeId.Equals(employee.Id)
                   && arg.SubstituteEmployeeId.Equals(substituteEmployee.Id)));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingId_ShouldThrowEmployeeNotFoundException()
    {
        //arrange
        var command = new DeactivateEmployeeCommand(Guid.NewGuid(), Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));

        //arrange
        exception.ShouldBeOfType<EmployeeNotFoundException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingIdAndNotExistingSubstitutionEmployeeId_ShouldThrowSubstituteEmployeeIdNotFoundException()
    {
        //arrange
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetEmployeeInCompany(company);

        _companyRepository
            .GetByEmployeeIdAsync(employee.Id)
            .Returns(company);
        var command = new DeactivateEmployeeCommand(employee.Id, Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));

        //arrange
        exception.ShouldBeOfType<SubstituteEmployeeIdNotFound>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotActiveSubstituteEmployeeId_ShouldThrowSubstituteEmployeeNotActiveException()
    {
        //arrange
        var company = CompanyFactory.Get();
        var employee = EmployeeFactory.GetEmployeeInCompany(company);
        var substituteEmployee = EmployeeFactory.GetEmployeeInCompany(company);
        substituteEmployee.Deactivate();

        _companyRepository
            .GetByEmployeeIdAsync(employee.Id)
            .Returns(company);
        var command = new DeactivateEmployeeCommand(employee.Id, substituteEmployee.Id);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //arrange
        exception.ShouldBeOfType<SubstituteEmployeeNotActiveException>();
    }
    
    #region arrange
    private readonly ICompanyRepository _companyRepository;
    private readonly IMessageBroker _messageBroker;
    private readonly DeactivateEmployeeCommandHandler _handler;

    public DeactivateEmployeeCommandHandlerTests()
    {
        _companyRepository = Substitute.For<ICompanyRepository>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new DeactivateEmployeeCommandHandler(_companyRepository, _messageBroker);
    }

    #endregion
}