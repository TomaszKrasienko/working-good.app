using NSubstitute;
using Shouldly;
using wg.modules.owner.application.CQRS.Owners.Commands.AddOwner;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Entities;
using wg.modules.owner.domain.Repositories;
using Xunit;

namespace wg.modules.owner.application.tests.CQRS.Owners.Commands;

public sealed class AddOwnerCommandHandlerTests
{
    private Task Act(AddOwnerCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenAddOwnerCommandForNotExistingOwner_ShouldAddOwnerByRepository()
    {
        //arrange
        var command = new AddOwnerCommand(Guid.NewGuid(), "WorkingGoodCompany");
        _ownerRepository
            .ExistsAsync()
            .Returns(false);
        
        //act
        await Act(command);
        
        //assert
        await _ownerRepository
            .Received(1)
            .AddAsync(Arg.Is<Owner>(x 
                => x.Id.Value == command.Id
                && x.Name.Value == command.Name));
    }

    [Fact]
    public async Task HandleAsync_GivenAddOwnerCommandForExistingOwner_ShouldThrowOwnerAlreadyExistsException()
    {
        //arrange
        var command = new AddOwnerCommand(Guid.NewGuid(), "WorkingGoodCompany");
        _ownerRepository
            .ExistsAsync()
            .Returns(true);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<OwnerAlreadyExistsException>();
    }
    
    #region arrange
    private readonly IOwnerRepository _ownerRepository;
    private readonly AddOwnerCommandHandler _handler;

    public AddOwnerCommandHandlerTests()
    {
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _handler = new AddOwnerCommandHandler(_ownerRepository);
    }
    #endregion
}