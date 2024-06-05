using Microsoft.AspNetCore.Routing.Matching;
using NSubstitute;
using Shouldly;
using wg.modules.wiki.application.Clients.Companies;
using wg.modules.wiki.application.Clients.Companies.DTOs;
using wg.modules.wiki.application.Clients.Tickets;
using wg.modules.wiki.application.Clients.Tickets.DTOs;
using wg.modules.wiki.application.CQRS.Notes.Commands;
using wg.modules.wiki.application.Exceptions;
using wg.modules.wiki.application.Strategies.Origins;
using wg.modules.wiki.core.Exceptions;
using wg.modules.wiki.domain.Exceptions;
using wg.modules.wiki.domain.Repositories;
using wg.modules.wiki.domain.ValueObjects.Note;
using wg.shared.abstractions.CQRS.Commands;
using wg.tests.shared.Factories.Wiki;
using Xunit;

namespace wg.modules.wiki.application.tests.CQRS.Notes.Commands;

public sealed class AddNoteCommandHandlerTests
{
    private Task Act(AddNoteCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenWithTicketOrigin_ShouldAddNoteToSectionAndUpdateSection()
    {
        //arrange
        var section = SectionsFactory.Get();
        var originId = Guid.NewGuid();
        var command = new AddNoteCommand(Guid.NewGuid(), "Title", "Content", section.Id,
            Origin.Ticket(), originId.ToString());
        
        _sectionRepository
            .GetByIdAsync(command.SectionId, default)
            .Returns(section);

        _ticketsApiClient
            .IsTicketExistsAsync(new TicketIdDto(originId))
            .Returns(new IsTicketExistsDto(true));
        
        //act
        await Act(command);
        
        //assert
        section.Notes.Any(x => x.Id.Equals(command.Id)).ShouldBeTrue();

        await _sectionRepository
            .Received(1)
            .UpdateAsync(section, default);


        await _ticketsApiClient
            .Received(1)
            .IsTicketExistsAsync(new TicketIdDto(originId));
    }

    [Fact]
    public async Task HandleAsync_GivenWithClientOrigin_ShouldAddNoteToSectionAndUpdateSection()
    {
        //arrange
        var section = SectionsFactory.Get();
        var originId = Guid.NewGuid();
        var command = new AddNoteCommand(Guid.NewGuid(), "Title", "Content", section.Id,
            Origin.Client(), originId.ToString());
        
        _sectionRepository
            .GetByIdAsync(command.SectionId, default)
            .Returns(section);

        _companiesApiClient
            .IsActiveCompanyExistsAsync(new CompanyIdDto(originId))
            .Returns(new IsActiveCompanyExistsDto(true));
        
        //act
        await Act(command);
        
        //assert
        section.Notes.Any(x => x.Id.Equals(command.Id)).ShouldBeTrue();

        await _sectionRepository
            .Received(1)
            .UpdateAsync(section, default);

        await _companiesApiClient
            .Received(1)
            .IsActiveCompanyExistsAsync(new CompanyIdDto(originId));
    }
    
    [Fact]
    public async Task HandleAsync_GivenWithoutOrigin_ShouldAddNoteToSectionAndUpdateSection()
    {
        //arrange 
        var section = SectionsFactory.Get();
        var command = new AddNoteCommand(Guid.NewGuid(), "Title", "Content", section.Id);
        
        _sectionRepository
            .GetByIdAsync(command.SectionId, default)
            .Returns(section);
        
        //act
        await Act(command);
        
        //assert
        section.Notes.Any(x => x.Id.Equals(command.Id)).ShouldBeTrue();

        await _sectionRepository
            .Received(1)
            .UpdateAsync(section, default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenTicketNotExistingOrigin_ShouldThrowOriginDoesNotExistException()
    {
        //arrange
        var section = SectionsFactory.Get();
        var originId = Guid.NewGuid();
        var command = new AddNoteCommand(Guid.NewGuid(), "Title", "Content", section.Id,
            Origin.Ticket(), originId.ToString());
        
        _sectionRepository
            .GetByIdAsync(command.SectionId, default)
            .Returns(section);

        _ticketsApiClient
            .IsTicketExistsAsync(new TicketIdDto(originId))
            .Returns(new IsTicketExistsDto(false));
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<OriginDoesNotExistException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenClientNotExistingOrigin_ShouldThrowOriginDoesNotExistException()
    {
        //arrange
        var section = SectionsFactory.Get();
        var originId = Guid.NewGuid();
        var command = new AddNoteCommand(Guid.NewGuid(), "Title", "Content", section.Id,
            Origin.Client(), originId.ToString());
        
        _sectionRepository
            .GetByIdAsync(command.SectionId, default)
            .Returns(section);

        _companiesApiClient
            .IsActiveCompanyExistsAsync(new CompanyIdDto(originId))
            .Returns(new IsActiveCompanyExistsDto(false));
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<OriginDoesNotExistException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingOriginType_ShouldReturnOriginTypeNoteAvailableException()
    {
        //arrange
        var section = SectionsFactory.Get();
        var command = new AddNoteCommand(Guid.NewGuid(), "Title", "Content", section.Id,
            "origin type", Guid.NewGuid().ToString());

        _sectionRepository
            .GetByIdAsync(command.SectionId, default)
            .Returns(section);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<OriginTypeNoteAvailableException>();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingSectionId_ShouldThrowSectionNotFoundException()
    {
        //arrange
        var command = new AddNoteCommand(Guid.NewGuid(), "Title", "Section",
            Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<SectionNotFoundException>();
    }
    
    #region arrange
    private readonly IEnumerable<IOriginCheckingStrategy> _originCheckingStrategies;
    private readonly ICompaniesApiClient _companiesApiClient;
    private readonly ITicketsApiClient _ticketsApiClient;
    private readonly ISectionRepository _sectionRepository;
    private readonly ICommandHandler<AddNoteCommand> _handler;

    public AddNoteCommandHandlerTests()
    {
        _sectionRepository = Substitute.For<ISectionRepository>();
        _companiesApiClient = Substitute.For<ICompaniesApiClient>();
        _ticketsApiClient = Substitute.For<ITicketsApiClient>();
        _originCheckingStrategies =
        [
            new ClientCheckingStrategy(_companiesApiClient), new TicketsCheckingStrategy(_ticketsApiClient)
        ];
        _handler = new AddNoteCommandHandler(_sectionRepository, _originCheckingStrategies);
    }
    #endregion
}