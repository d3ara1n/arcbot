using System;
using Hyperai.Units;
using HyperaiShell.Foundation.Plugins;
using Microsoft.Extensions.Configuration;

namespace Arcbot.Essential.Services
{
    public class ReportService
    {
        private readonly long _daddy;

        private bool enabled = false;
        public ReportService(IConfiguration configuration, IPluginConfiguration<PluginEntry> pluginConfiguration)
        {
            _daddy = Convert.ToInt64(configuration["Application:Daddy"]);
            enabled = Convert.ToBoolean(pluginConfiguration.Value["Services:ReportService:Enabled"]);
        }

        public void Push(MessageContext context)
        {
            if(!enabled) return;
        }
    }
}