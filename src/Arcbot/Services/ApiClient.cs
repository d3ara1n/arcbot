using System;
using System.Threading;
using HyperaiX;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Onebot.Protocol;

namespace Arcbot.Services;

public class ApiClient : IApiClient
{
    private readonly ILogger _logger;
    private readonly ApiClientOptions _options;

    internal readonly OnebotClient client;

    public ApiClient(ILogger<ApiClient> logger, IOptions<ApiClientOptions> options)
    {
        _logger = logger;
        _options = options.Value;

        client = new OnebotClient(ConnectionFactory.FromWebsocket(_options.Host, _options.Port, _options.AccessToken));
    }

    public GenericEventArgs Read(CancellationToken token)
    {
        var result = client.Connection.FetchAsync(token).GetAwaiter().GetResult();
        try
        {
            return result.ToHyperai(client);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Error occured while parsing event: {Event}", result);
            throw;
        }
    }

    public GenericReceipt Write(GenericActionArgs action)
    {
        switch (action)
        {
            case QueryMemberActionArgs it:
            {
                var member = client
                    .GetHyperaiMemberAsync(it.GroupId, it.MemberId).Result;
                return new QueryMemberReceipt
                {
                    Member = member,
                    Success = member != null
                };
            }
            case QueryGroupActionArgs it:
            {
                var group = client.GetHyperaiGroupAsync(it.GroupId).Result;
                return new QueryGroupReceipt
                {
                    Group = group,
                    Success = group != null
                };
            }
            case FriendMessageActionArgs it:
            {
                var receipt = client.SendPrivateMessageAsync(it.FriendId.ToString(), it.Message.ToOnebot(client))
                    .Result;
                return new MessageReceipt
                {
                    MessageId = receipt?.MessageId,
                    Success = receipt != null
                };
            }
            case GroupMessageActionArgs it:
            {
                var receipt = client.SendGroupMessageAsync(it.GroupId.ToString(), it.Message.ToOnebot(client))
                    .Result;
                return new MessageReceipt
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