using System;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Services;
using HyperaiShell.Foundation.ModelExtensions;
using Microsoft.Extensions.Configuration;

namespace Arcbot.Essential.Services
{
    public class ReportService
    {
        private readonly IApiClient _client;
        private readonly IConfiguration _configuration;
        private readonly Friend _daddy;

        public ReportService(IConfiguration configuration, IApiClient client)
        {
            _configuration = configuration;
            _daddy = new Friend()
            {
                Identity = long.Parse(configuration["Application:Daddy"])
            };
            _client = client;
        }

        public void Report(Action<MessageChainBuilder> message)
        {
            var chainBuilder = new MessageChainBuilder();
            message(chainBuilder);
            _daddy.SendAsync(chainBuilder.Build());
        }

        public void OnStarted()
        {
            Report(builder => builder.AddPlain($"表锅，我粗来了哦。我在 {Environment.UserName}@{Environment.MachineName} 上了哦"));
        }
    }
}