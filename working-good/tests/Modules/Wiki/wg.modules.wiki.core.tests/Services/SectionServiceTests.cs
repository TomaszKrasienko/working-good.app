using NSubstitute;
using Shouldly;
using wg.modules.wiki.core.DAL.Repositories.Abstractions;
using wg.modules.wiki.core.Entities;
using wg.modules.wiki.core.Exceptions;
using wg.modules.wiki.core.Services.Abstractions;
using wg.modules.wiki.core.Services.Commands;
using wg.modules.wiki.core.Services.Internals;
using wg.tests.shared.Factories.Wiki;
using Xunit;

namespace wg.modules.wiki.core.tests.Services;

public sealed class SectionServiceTests
{
    [Fact]
    public async Task AddAsync_GivenCommandWithParentId_ShouldAddSectionByRepository()
    {
        //arrange
        var parent = SectionsFactory.Get();
        var command = new AddSectionCommand(Guid.NewGuid(), "Section name", parent.Id);

        _sectionRepository
            .GetByIdAsync(command.ParentId!.Value, default)
            .Returns(parent);

        _sectionRepository
            .IsNameExistsAsync(command.Name, default)
            .Returns(false);
        
        //act
        await _sectionService.AddAsync(command, default);
        
        //assert
        await _sectionRepository
            .Received(1)
            .AddAsync(Arg.Is<Section>(arg
                => arg.Id.Equals(command.Id)
                && arg.Name == command.Name
                && arg.Parent == parent), default);
    }
    
    [Fact]
    public async Task AddAsync_GivenCommandWithout_ShouldAddSectionByRepository()
    {
        //arrange
        var command = new AddSectionCommand(Guid.NewGuid(), "Section name", null);
        
        _sectionRepository
            .IsNameExistsAsync(command.Name, default)
            .Returns(false);
        
        //act
        await _sectionService.AddAsync(command, default);
        
        //assert
        await _sectionRepository
            .Received(1)
            .AddAsync(Arg.Is<Section>(arg
                => arg.Id.Equals(command.Id)
                   && arg.Name == command.Name
                   && arg.Parent == null), default);

        await _sectionRepository
            .Received(0)
            .GetByIdAsync(Arg.Any<Guid>(), default);
    }

    [Fact]
    public async Task AddAsync_GivenAlreadyRegisteredName_ShouldThrowSectionNameAlreadyRegisteredException()
    {
        //arrange
        var command = new AddSectionCommand(Guid.NewGuid(), "Section name", null);
        
        _sectionRepository
            .IsNameExistsAsync(command.Name, default)
            .Returns(true);
            
        //act
        var exception = await Record.ExceptionAsync(async () => await _sectionService.AddAsync(command, default));
        
        //assert
        exception.ShouldBeOfType<SectionNameAlreadyRegisteredException>();
    }
    
    [Fact]
    public async Task AddAsync_GivenCommandWithNotExistingParentId_ShouldThrowParentSectionNotFoundException()
    {
        //arrange
        var command = new AddSectionCommand(Guid.NewGuid(), "Section name", Guid.NewGuid());
        
        _sectionRepository
            .IsNameExistsAsync(command.Name, default)
            .Returns(false);
        //act
        var exception = await Record.ExceptionAsync(async () => await _sectionService.AddAsync(command, default));
        
        //assert
        exception.ShouldBeOfType<ParentSectionNotFoundException>();
    }

    #region arrange

    private readonly ISectionRepository _sectionRepository;
    private readonly ISectionService _sectionService;

    public SectionServiceTests()
    {
        _sectionRepository = Substitute.For<ISectionRepository>();
        _sectionService = new SectionService(_sectionRepository);
    }
    #endregion
}