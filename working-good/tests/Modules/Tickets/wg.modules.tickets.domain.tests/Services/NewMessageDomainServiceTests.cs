using NSubstitute;
using wg.modules.tickets.domain.Entities;
using wg.modules.tickets.domain.Repositories;
using wg.modules.tickets.domain.Services;
using Xunit;

namespace wg.modules.tickets.domain.tests.Services;

public class NewMessageDomainServiceTests
{
    [Fact]
    public async Task AddNewMessage_GivenNullNumberAndId_ShouldAddNewTicketByRepository()
    {
        //arrange
        var id = Guid.NewGuid();
        var sender = "joe.doe@test.pl";
        var subject = "My Test Subject";
        var content = "My Test Content";
        var createdAt = DateTime.Now;
        
        //act
        await _service.AddNewMessage(id, sender, subject, content, createdAt, null, null);
        
        //assert
        await _ticketRepository
            .Received(1)
            .AddAsync(Arg.Is<Ticket>(x
                => x.Id.Equals(id)
                   && x.Subject == subject
                   && x.Content == content
                   && x.CreatedAt.Equals(createdAt)));
    }
    
    #region arrange
    private readonly ITicketRepository _ticketRepository;
    private readonly NewMessageDomainService _service;
    
    public NewMessageDomainServiceTests()
    {
        _ticketRepository = Substitute.For<ITicketRepository>();
        _service = new NewMessageDomainService(_ticketRepository);
    }
    #endregion
}