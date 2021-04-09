using Arcbot.Essential.Bots;
using Arcbot.Essential.Services;
using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcbot.Essential
{
    public class PluginEntry : PluginBase
    {
        public override void ConfigureBots(IBotCollectionBuilder bots, IConfiguration config)
        {
            bots.Add<EchoBot>();
            bots.Add<RepeaterBot>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<EchoService>();
            services.AddSingleton<ProfileService>();
        }

        public override void PostConfigure(IConfiguration config)
        {
        }
    }
}