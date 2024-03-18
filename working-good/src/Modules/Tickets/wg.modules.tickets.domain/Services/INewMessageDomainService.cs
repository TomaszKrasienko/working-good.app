namespace wg.modules.tickets.domain.Services;

public interface INewMessageDomainService
{
    Task AddNewMessage(Guid id, string sender, string subject, string content,
        DateTime createdAt, int? ticketNumber, Guid? ticketId, Guid? employeeId);
}