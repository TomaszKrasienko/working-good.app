using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.application.Exceptions;

public sealed class OriginDoesNotExistException(string originId, string originType)
    : WgException($"Origin with ID: {originId} and Type: {originType} does not exist");