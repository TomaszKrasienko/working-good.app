using wg.bootstrapper;
using wg.shared.infrastructure.Configuration;
using wg.shared.infrastructure.Modules.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Host.ConfigureModules();
builder.Services.AddHealthChecks();
var assemblies = ModuleLoader.GetAssemblies(builder.Configuration);
var modules = ModuleLoader.GetModules(assemblies);
builder.Services.AddModulesConfiguration(modules);
builder.Services.AddInfrastructure(assemblies, builder.Configuration);
var app = builder.Build();
app.UseHttpsRedirection();
app.MapHealthChecks("/wg");
app.UseInfrastructure();
app.Run();