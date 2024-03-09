namespace wg.sharedForTests.Factories.Owner;

public static class OwnerFactory
{
    public static modules.owner.domain.Entities.Owner Get()
        => modules.owner.domain.Entities.Owner.Create(Guid.NewGuid(), "MyOwnerComp");
}