using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace wg.shared.abstractions.Modules;

public interface IModule
{
    string Name { get; }
    IEnumerable<string> Policies => null;
    void Register(IServiceCollection services);
    void Use(WebApplication app);
}