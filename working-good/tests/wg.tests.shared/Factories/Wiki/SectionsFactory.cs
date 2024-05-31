using Bogus;
using wg.modules.wiki.core.Entities;

namespace wg.tests.shared.Factories.Wiki;

internal static class SectionsFactory
{
    internal static Section Get(Section parent = null)
        => Get(1, parent).Single();
    
    internal static List<Section> Get(int count, Section parent)
        => GetFaker(parent).Generate(count);
    
    private static Faker<Section> GetFaker(Section parent)
        => new Faker<Section>()
            .CustomInstantiator(v => Section.Create(
                Guid.NewGuid(),
                v.Name.FindName(),
                parent));
}