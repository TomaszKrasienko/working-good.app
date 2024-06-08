using wg.bootstrapper;
using wg.shared.infrastructure.Configuration;
using wg.shared.infrastructure.Modules.Configuration;
using wg.shared.infrastructure.Vault.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Host.AddVault(builder.Configuration);
builder.Host.ConfigureModules();
builder.Services.AddHealthChecks();
var assemblies = ModuleLoader.GetAssemblies(builder.Configuration);
var modules = ModuleLoader.GetModules(assemblies);
builder.Services.AddInfrastructure(assemblies, builder.Configuration);
builder.Services.AddModulesConfiguration(modules);
builder.UseInfrastructure();
var app = builder.Build();

app.UseHttpsRedirection();
app.MapHealthChecks("/wg");
app.UseInfrastructure();
app.UseModulesConfiguration(modules);
app.Run();