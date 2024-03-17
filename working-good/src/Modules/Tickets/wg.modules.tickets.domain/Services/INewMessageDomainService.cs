namespace wg.modules.tickets.domain.Services;

public interface INewMessageDomainService
{
    Task AddNewMessage(Guid messageId, string sender, string subject, string content,
        DateTime createdAt, int? ticketNumber, Guid? ticketId);
}