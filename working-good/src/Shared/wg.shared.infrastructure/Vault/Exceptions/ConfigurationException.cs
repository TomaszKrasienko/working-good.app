namespace wg.shared.infrastructure.Vault.Exceptions;

public sealed class ConfigurationException(string message)
    : Exception(message);