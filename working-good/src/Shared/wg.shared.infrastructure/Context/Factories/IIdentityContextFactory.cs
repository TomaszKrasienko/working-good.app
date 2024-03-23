using wg.shared.abstractions.Context;

namespace wg.shared.infrastructure.Context.Factories;

public interface IIdentityContextFactory
{
    IIdentityContext Create();
}