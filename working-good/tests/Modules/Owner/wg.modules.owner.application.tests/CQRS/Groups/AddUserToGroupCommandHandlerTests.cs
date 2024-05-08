using NSubstitute;
using Shouldly;
using wg.modules.owner.application.CQRS.Groups.Commands.AddUserToGroup;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.modules.owner.domain.ValueObjects.User;
using wg.tests.shared.Factories.Owners;
using Xunit;

namespace wg.modules.owner.application.tests.CQRS.Groups;

public sealed class AddUserToGroupCommandHandlerTests
{
    private Task Act(AddUserToGroupCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_ForExistingOwner_ShouldUpdateOwnerByRepository()
    {
        //arrange
        var owner = OwnerFactory.Get();
        var user = UserFactory.GetInOwner(owner, Role.Manager());
        var group = GroupFactory.GetInOwner(owner);
        _ownerRepository
            .GetAsync()
            .Returns(owner);
        var command = new AddUserToGroupCommand(group.Id, user.Id);
        
        //act
        await Act(command);
        
        //assert
        group.Users.Any(x => x.Id == user.Id).ShouldBeTrue();
        await _ownerRepository
            .Received(1)
            .UpdateAsync(owner);
    }
    
    [Fact]
    public async Task HandleAsync_ForNotExistingOwner_ShouldThrowOwnerNotFoundException()
    {
        //arrange
        var command = new AddUserToGroupCommand(Guid.NewGuid(), Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<OwnerNotFoundException>();
    }
    
    #region arrange
    private readonly IOwnerRepository _ownerRepository;
    private readonly AddUserToGroupCommandHandler _handler;

    public AddUserToGroupCommandHandlerTests()
    {
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _handler = new AddUserToGroupCommandHandler(_ownerRepository);
    }
    #endregion
}