namespace wg.modules.messages.core.Services.Commands;

public sealed record CreateMessage(string Email, string Subject, string Content, int? TicketNumber);