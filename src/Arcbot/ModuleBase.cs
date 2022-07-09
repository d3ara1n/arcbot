using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot;

public abstract class ModuleBase
{
    public abstract IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration);
}