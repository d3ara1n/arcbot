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
            return socket.ReadAsync(CancellationToken.None).GetAwaiter().GetResult().ToEventArgs();
        }

        public GenericReceipt Write(GenericActionArgs action)
        {
            return socket.WriteAsync(action.ToAction()).GetAwaiter().GetResult().ToReceipt();
        }
    }
}