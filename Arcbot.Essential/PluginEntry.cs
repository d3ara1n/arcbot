using System;
using System.Net.Http.Headers;
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
            bots.Add<ReplyBot>();
            bots.Add<ForwardBot>();
            bots.Add<ReportBot>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<EchoService>();
            services.AddSingleton<ProfileService>();
            services.AddSingleton<ReplyService>();
            services.AddSingleton<ReportService>();
        }

        public override void OnStarted(IServiceProvider provider , IConfiguration config)
        {
            var service = provider.GetRequiredService<ReportService>();
            service.OnStarted();
        }
    }
}