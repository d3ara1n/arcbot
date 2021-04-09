using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.ClassTable
{
    public class PluginEntry : PluginBase
    {
        public override void ConfigureBots(IBotCollectionBuilder bots, IConfiguration config)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
        }

        public override void PostConfigure(IConfiguration config)
        {
        }
    }
}