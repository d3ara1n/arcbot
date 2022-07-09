using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.Modules.Github;

public class GithubModule: ModuleBase
{
    public override IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration) =>
        services;
}