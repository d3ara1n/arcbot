using Ac682.Hyperai.Plugins.Essential.Bots;
using Ac682.Hyperai.Plugins.Essential.Services;
using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Ac682.Hyperai.Plugins.Essential
{
    public class PluginEntry : PluginBase
    {
        public override void ConfigureBots(IBotCollectionBuilder bots)
        {
            bots.Add<EchoBot>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<EchoService>();
        }
    }
}
