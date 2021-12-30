using System;
using System.Threading;
using System.Threading.Tasks;
using Arcbot.Extensions;
using HyperaiX;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Clients;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Onebot.Protocol.Communications;

namespace Arcbot.Services
{
    public class OnebotClient: IApiClient
    {
        private readonly OnebotClientOptions _options;
        private readonly ILogger _logger;
        
        private OnebotWebsocket socket;
        private CancellationTokenSource tokenSource = new();

        public OnebotClient(IOptions<OnebotClientOptions> options, ILogger<OnebotClient> logger)
        {
            _options = options.Value;
            _logger = logger;
            
            socket = OnebotWebsocket.Connect(_options.Host, _options.Port, _options.AccessToken);
        }

        public GenericEventArgs Read()
        {
            return socket.Read(CancellationToken.None).ToEventArgs() switch
            {
                // apply Group info to GroupRelatedEventArgs
                GenericEventArgs it => it,
            };
        }

        public GenericReceipt Write(GenericActionArgs action)
        {
            _logger.LogDebug("Write Operation: {Action}", action.ToAction());
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            return socket.Write(action.ToAction(), source.Token).ToReceipt();
        }
    }
}