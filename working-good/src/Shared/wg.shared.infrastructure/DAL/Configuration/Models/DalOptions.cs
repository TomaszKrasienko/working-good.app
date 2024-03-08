namespace wg.shared.infrastructure.DAL.Configuration.Models;

internal sealed record DalOptions
{
    public string ConnectionString { get; init; }
}