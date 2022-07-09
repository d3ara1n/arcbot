using Arcbot.Modules.MentionForward.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.Modules.MentionForward;

public class MentionForwardModule: ModuleBase
{
    public override IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration) =>
        services.Configure<MentionForwardOptions>(configuration.GetSection("MentionForward"));
}