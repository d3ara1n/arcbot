using Arcbot.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.Modules.Arcbot;

public class ArcbotModule : ModuleBase
{
    public override IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<ArcbotOptions>(configuration.GetSection("Arcbot"));
    }
}