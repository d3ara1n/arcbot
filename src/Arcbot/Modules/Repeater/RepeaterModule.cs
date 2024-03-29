using Arcbot.Modules.Repeater.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.Modules.Repeater;

public class RepeaterModule : ModuleBase
{
    public override IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<RepeaterOptions>(configuration.GetSection("Repeater"));
    }
}