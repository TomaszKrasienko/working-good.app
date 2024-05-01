using NSubstitute;
using Shouldly;
using wg.modules.messages.core.Clients.Companies;
using wg.modules.messages.core.Clients.Companies.DTO;
using wg.modules.messages.core.Events;
using wg.modules.messages.core.Exceptions;
using wg.modules.messages.core.Services;
using wg.modules.messages.core.Services.Abstractions;
using wg.modules.messages.core.Services.Commands;
using wg.shared.abstractions.Messaging;
using wg.tests.shared.Factories.DTOs.Messages;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.messages.core.tests.Services;

public sealed class MessageServiceTests
{
    [Fact]
    public async Task CreateMessage_GivenExistingActiveEmployeeWithNullTicketNumber_ShouldSendMessageReceivedEvent()
    {
        //arrange
        var employeeDto = EmployeeDtoFactory.Get(1).Single();
        var command = new CreateMessage(employeeDto.Email, "My test ticket",
            "My test content", null);
        _companiesApiClient
            .GetEmployeeByEmailAsync(Arg.Is<EmployeeEmailDto>(arg => arg.Email == command.Email))
            .Returns(employeeDto);
        
        //act
        await _messageService.CreateMessage(command);
        
        //assert
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<MessageReceived>(arg
                => arg.CreatedAt == _now
                   && arg.Sender == command.Email
                   && arg.Subject == command.Subject
                   && arg.Content == command.Content
                   && arg.AssignedEmployee == employeeDto.Id
                   && arg.TicketNumber == null));
    }
    
    [Fact]
    public async Task CreateMessage_GivenExistingActiveEmployee_ShouldSendMessageReceivedEvent()
    {
        //arrange
        var employeeDto = EmployeeDtoFactory.Get(1).Single();
        var command = new CreateMessage(employeeDto.Email, "My test ticket",
            "My test content", 1);
        _companiesApiClient
            .GetEmployeeByEmailAsync(Arg.Is<EmployeeEmailDto>(arg => arg.Email == command.Email))
            .Returns(employeeDto);
        
        //act
        await _messageService.CreateMessage(command);
        
        //assert
        await _messageBroker
            .Received(1)
            .PublishAsync(Arg.Is<MessageReceived>(arg
                => arg.CreatedAt == _now
                   && arg.Sender == command.Email
                   && arg.Subject == command.Subject
                   && arg.Content == command.Content
                   && arg.AssignedEmployee == employeeDto.Id
                   && arg.TicketNumber == command.TicketNumber));
    }
    
    [Fact]
    public async Task CreateMessage_GivenNotExistingEmployee_ShouldThrowEmployeeNotFoundException()
    {
        //arrange
        var employeeDto = EmployeeDtoFactory.Get(1).Single();
        var command = new CreateMessage(employeeDto.Email, "My test ticket",
            "My test content", 1);
        
        //act
        var exception = await Record.ExceptionAsync(async() => await _messageService.CreateMessage(command));
        
        //assert
        exception.ShouldBeOfType<EmployeeNotFoundException>();
    }

    #region arrange
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly DateTime _now;
    private readonly IMessageBroker _messageBroker;
    private readonly IMessageService _messageService;

    public MessageServiceTests()
    {
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _now = DateTime.Now;
        _messageService = new MessageService(_companiesApiClient, TestsClock.Create(_now), _messageBroker);
    }

    #endregion
}