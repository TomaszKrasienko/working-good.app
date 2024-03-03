using System.Reflection;

namespace wg.bootstrapper;

internal static class ModuleLoader
{
    internal static IList<Assembly> LoadAssemblies(IConfiguration configuration)
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
}