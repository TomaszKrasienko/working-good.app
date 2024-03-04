using wg.bootstrapper;
using wg.shared.infrastructure.Configuration;
using wg.shared.infrastructure.Modules.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Host.ConfigureModules();
var assemblies = ModuleLoader.GetAssemblies(builder.Configuration);
var modules = ModuleLoader.GetModules(assemblies);
builder.Services.AddModulesConfiguration(modules);
builder.Services.AddInfrastructure(assemblies);
var app = builder.Build();
app.UseHttpsRedirection();
app.UseInfrastructure();
app.Run();