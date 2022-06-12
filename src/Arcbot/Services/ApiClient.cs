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

namespace Arcbot.Services
{
    public class ApiClient : IApiClient
    {
        readonly ILogger _logger;
        readonly OnebotClient client;

        public ApiClient(ILogger<ApiClient> logger)
        {
            _logger = logger;

            client = new OnebotClient(ConnectionFactory.FromWebsocket());
        }

        public GenericEventArgs Read()
        {
            
            throw new NotImplementedException();
        }

        public GenericReceipt Write(GenericActionArgs action)
        {
            return new GenericReceipt();
        }
    }
}