using System.Collections;
using System.Reflection;
using wg.bootstrapper;
using wg.shared.abstractions.Modules;
using wg.shared.infrastructure.Configuration;
using wg.shared.infrastructure.Modules.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Host.ConfigureModules();
IList<Assembly> assemblies = ModuleLoader.GetAssemblies(builder.Configuration);
IList<IModule> modules = ModuleLoader.GetModules(assemblies);
builder.Services.AddInfrastructure(assemblies);
var app = builder.Build();
app.UseHttpsRedirection();
app.Run();