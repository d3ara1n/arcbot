using Arcbot.Essential.Bots;
using Arcbot.Essential.Services;
using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.Essential
{
    public class PluginEntry : PluginBase
    {
        public override void ConfigureBots(IBotCollectionBuilder bots)
        {
            bots.Add<EchoBot>();
            bots.Add<RecordBot>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<EchoService>();
            services.AddSingleton<RecordService>();
        }
    }
}
