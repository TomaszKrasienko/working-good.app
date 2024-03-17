using wg.shared.abstractions.Exceptions;

namespace wg.modules.tickets.domain.Exceptions;

public sealed class EmptySubjectException()
    : WgException("Subject can not be empty");