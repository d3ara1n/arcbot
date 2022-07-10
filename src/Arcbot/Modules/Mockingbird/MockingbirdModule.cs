using Arcbot.Modules.Mockingbird.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.Modules.Mockingbird;

public class MockingbirdModule : ModuleBase
{
    public override IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<MockingbirdOptions>(configuration.GetSection("Mockingbird"));
    }
}