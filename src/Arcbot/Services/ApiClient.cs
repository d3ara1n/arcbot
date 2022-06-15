using System;
using System.Threading;
using HyperaiX;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Abstractions.Relations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Onebot.Protocol;
using Onebot.Protocol.Models.Events.Meta;

namespace Arcbot.Services
{
    public class ApiClient : IApiClient
    {
        private readonly ILogger _logger;
        private readonly ApiClientOptions _options;
        
        readonly OnebotClient client;

        public ApiClient(ILogger<ApiClient> logger, IOptions<ApiClientOptions> options)
        {
            _logger = logger;
            _options = options.Value;

            client = new OnebotClient(ConnectionFactory.FromWebsocket(_options.Host, _options.Port, _options.AccessToken));
        }

        public GenericEventArgs Read(CancellationToken token)
        {
            var result = client.Connection.FetchAsync(token).GetAwaiter().GetResult();
            return result.ToHyperai();
        }

        public GenericReceipt Write(GenericActionArgs action)
        {
            throw new NotImplementedException();
        }
    }
}