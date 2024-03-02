using System.Dynamic;
using wg.modules.owner.domain.Entities;

namespace wg.modules.owner.tests.shared.Factories;

internal static class OwnerFactory
{
    internal static Owner Get()
        => Owner.Create(Guid.NewGuid(), "MyOwnerComp");
}