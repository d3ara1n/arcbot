using System.Diagnostics;
using HyperaiX.Abstractions.Modules;
using HyperaiX.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.Modules.Debugging;

public class DebugModule : ModuleBase
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        Debug.WriteLine(configuration["WelcomeString"]);
    }
}