using wg.shared.abstractions.Exceptions;

namespace wg.modules.wiki.core.Exceptions;

public sealed class SectionNameAlreadyRegisteredException(string name)
    : WgException($"Section name: {name} is already registered");