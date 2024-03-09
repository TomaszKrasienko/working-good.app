using Microsoft.Extensions.Configuration;

namespace wg.sharedForTests.Integration;

public sealed class OptionsProvider
{
    private readonly IConfiguration _configuration;

    public OptionsProvider()
    {
        _configuration = GetConfigurationRoot();
    }

    private static IConfigurationRoot GetConfigurationRoot()
        => new ConfigurationBuilder()
            .AddJsonFile("appsettings.tests.json")
            .AddEnvironmentVariables()
            .Build();

    public T Get<T>(string section) where T : class, new()
    {
        T t = new T();
        _configuration.Bind(section, t);
        return t;
    }
}