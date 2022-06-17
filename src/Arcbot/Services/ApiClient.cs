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
        private const int TIMEOUT = 1500;

        public ApiClient(ILogger<ApiClient> logger, IOptions<ApiClientOptions> options)
        {
            _logger = logger;
            _options = options.Value;

            client = new OnebotClient(ConnectionFactory.FromWebsocket(_options.Host, _options.Port, _options.AccessToken));
        }

        public GenericEventArgs Read(CancellationToken token)
        {
            var result = client.Connection.FetchAsync(token).GetAwaiter().GetResult();
            return result.ToHyperai(client);
        }

        public GenericReceipt Write(GenericActionArgs action)
        {
            switch (action)
            {
                case QueryMemberActionArgs it:
                {
                    var member = client
                        .GetHyperaiMemberAsync(it.GroupId, it.MemberId).Result;
                    return new QueryMemberReceipt()
                    {
                        Member = member,
                        Success = member != null
                    };
                }
                case QueryGroupActionArgs it:
                {
                    var group = client.GetHyperaiGroupAsync(it.GroupId).Result;
                    return new QueryGroupReceipt()
                    {
                        Group = group,
                        Success = group != null
                    };
                }
                case FriendMessageActionArgs it:
                {
                    var receipt = client.SendPrivateMessageAsync(it.FriendId.ToString(), it.Message.ToOnebot(client))
                        .Result;
                    return new MessageReceipt()
                    {
                        MessageId = receipt?.MessageId,
                        Success = receipt != null
                    };
                }
                case GroupMessageActionArgs it:
                {
                    var receipt = client.SendGroupMessageAsync(it.GroupId.ToString(), it.Message.ToOnebot(client))
                        .Result;
                    return new MessageReceipt()
                    {
                        MessageId = receipt?.MessageId,
                        Success = receipt != null
                    };
                }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}