using NSubstitute;
using Shouldly;
using wg.modules.owner.application.CQRS.Users.Commands.VerifyUser;
using wg.modules.owner.application.Exceptions;
using wg.modules.owner.domain.Repositories;
using wg.modules.owner.domain.ValueObjects.User;
using wg.shared.abstractions.Time;
using wg.tests.shared.Factories.Owners;
using wg.tests.shared.Mocks;
using Xunit;

namespace wg.modules.owner.application.tests.CQRS.Users.Commands;

public sealed class VerifyUserCommandHandlerTests
{
    private Task Act(VerifyUserCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenVerifyUserCommandForExistingOwner_ShouldUpdateOwnerAndChangeUserState()
    {
        //arrange
        var owner = OwnerFactory.Get();
        UserFactory.GetInOwner(owner, Role.Manager());
        var user = owner.Users.Single();
        var command = new VerifyUserCommand(user.VerificationToken.Token);
        
        _ownerRepository
            .GetAsync()
            .Returns(owner);
        
        //act
        await Act(command);
        
        //assert
        await _ownerRepository
            .Received(1)
            .UpdateAsync(owner);
        user.State.Value.ShouldBe("Active");
    }
    
    [Fact]
    public async Task HandleAsync_GivenVerifyUserCommandForNotExistingOwner_ShouldThrowOwnerNotFoundException()
    {
        //arrange
        var command = new VerifyUserCommand(Guid.NewGuid().ToString("N"));
        await _ownerRepository
            .GetAsync();
        
        //act
        var exception = await Record.ExceptionAsync(async() => await Act(command));
        
        //assert
        exception.ShouldBeOfType<OwnerNotFoundException>();
    }
    
    #region arrange
    private readonly IOwnerRepository _ownerRepository;
    private readonly IClock _clock;
    private readonly VerifyUserCommandHandler _handler;
    
    public VerifyUserCommandHandlerTests()
    {
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _clock = TestsClock.Create();
        _handler = new VerifyUserCommandHandler(_ownerRepository, _clock);
    }
    #endregion
}