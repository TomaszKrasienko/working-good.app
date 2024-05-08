using wg.modules.owner.domain.Entities;

namespace wg.tests.shared.Factories.Owners;

public static class OwnerFactory
{
    public static Owner Get()
        => Owner.Create(Guid.NewGuid(), "MyOwnerComp");
}