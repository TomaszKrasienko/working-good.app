using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using wg.shared.abstractions.Mailbox;
using wg.shared.infrastructure.Mailbox.Configuration.Models;

namespace wg.shared.infrastructure.Mailbox.Configuration;

internal static class Extensions
{
    private const string SectionName = "Mailbox";
    
    internal static IServiceCollection AddMailbox(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddRegister();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<MailboxOptions>(configuration.GetSection(SectionName));

    private static IServiceCollection AddRegister(this IServiceCollection services)
        => services.AddSingleton<IMailboxRegister, MailboxRegister>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MailboxOptions>>();
            return new MailboxRegister(options);
        });
}