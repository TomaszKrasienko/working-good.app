using System.Reflection;
using wg.shared.abstractions.Modules;

namespace wg.bootstrapper;

internal static class ModuleLoader
{
    internal static IList<Assembly> GetAssemblies(IConfiguration configuration)
    {
        const string modulePartPrefix = "wg.modules";
        var assemblies = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .ToList();

        var locations = assemblies
            .Where(x => !x.IsDynamic)
            .Select(x => x.Location)
            .ToArray();

        var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(x => !locations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
            .ToList();
        var disabledModules = new List<string>();
        foreach (var file in files)
        {
            if (!file.Contains(modulePartPrefix))
            {
                continue;
            }

            var moduleName = file.Split(modulePartPrefix)[1].Split(".")[0].ToLowerInvariant();
            var enabled = configuration.GetValue<bool>($"{moduleName}:module:enabled");
            if (enabled)
            {
                disabledModules.Add(file);
            }
        }

        foreach (var module in disabledModules)
        {
            files.Remove(module);
        }
        files.ForEach(x => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));
        return assemblies;
    }

    internal static IList<IModule> GetModules(IEnumerable<Assembly> assemblies)
        => assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IModule).IsAssignableFrom(x) && !x.IsInterface)
            .OrderBy(x => x.Name)
            .Select(Activator.CreateInstance)
            .Cast<IModule>()
            .ToList();
}