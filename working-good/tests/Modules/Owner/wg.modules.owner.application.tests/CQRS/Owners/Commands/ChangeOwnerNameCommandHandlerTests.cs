using NSubstitute;
using Shouldly;
using wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;
using wg.modules.owner.application.CQRS.Owners.Commands.ChangeOwnerName;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.Repositories;
using wg.modules.owner.tests.shared.Factories;
using wg.sharedForTests.Factories.Owner;
using Xunit;

namespace wg.modules.owner.application.tests.CQRS.Owners.Commands;

public sealed class ChangeOwnerNameCommandHandlerTests
{
    private Task Act(ChangeOwnerNameCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenExistingOwnerName_ShouldUpdateNameAndSaveByRepository()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var command = new ChangeOwnerNameCommand(owner.Id, Guid.NewGuid().ToString("N"));
        _ownerRepository
            .GetAsync()
            .Returns(owner);
        
        //act
        await Act(command);
        
        //assert
        await _ownerRepository
            .Received(1)
            .UpdateAsync(Arg.Is<Owner>(arg
                => arg.Id.Value == command.Id
                   && arg.Name.Value == command.Name));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingOwnerName_ShouldThrowOwnerNotFoundException()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var command = new ChangeOwnerNameCommand(owner.Id, Guid.NewGuid().ToString("N"));
        await _ownerRepository
            .GetAsync();
        
        //act
        var exception = await Record.ExceptionAsync(async() => await Act(command));
        
        //assert
        exception.ShouldBeOfType<OwnerNotFoundException>();
    }
    
    #region arrange
    private readonly IOwnerRepository _ownerRepository;
    private readonly ChangeOwnerNameCommandHandler _handler;

    public ChangeOwnerNameCommandHandlerTests()
    {
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _handler = new ChangeOwnerNameCommandHandler(_ownerRepository);
    }
    #endregion
}