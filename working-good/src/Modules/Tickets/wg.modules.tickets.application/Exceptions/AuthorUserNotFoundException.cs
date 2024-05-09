using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.application.Exceptions;

public sealed class AuthorUserNotFoundException(Guid id) 
    : WgException($"Author user with ID:{id} was not found");