using System.Linq;
using System.Threading.Tasks;
using Arcbot.Modules.MentionForward.Options;
using HyperaiX;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;
using Microsoft.Extensions.Options;

namespace Arcbot.Modules.MentionForward.Units;

public class ForwardUnit : UnitBase
{
    private readonly MentionForwardOptions _options;
    private readonly IApiClient _client;

    public ForwardUnit(IOptions<MentionForwardOptions> options, IApiClient client)
    {
        _options = options.Value;
        _client = client;
    }

    [Receiver(MessageEventType.Group)]
    public async void Forward(MessageChain chain, Group group, Member sender)
    {
        var identity = (await _client.GetSelfInfoAsync()).Identity;
        if (!chain.Any(x => x is At at && at.Identity == identity)) return;
        await _client.SendFriendMessageAsync(_options.Destination,
            MessageChain.Construct(new Plain(
                $"Message from Group {sender.DisplayName}({sender.Identity})@{group.Name}({group.Identity}): ")));
        await _client.SendFriendMessageAsync(_options.Destination,
            new MessageChain(chain.Select(x => x switch { At => new Plain("@Me"), _ => x })));
    }

    [Receiver(MessageEventType.Friend)]
    public async void Forward(MessageChain chain, Friend friend)
    {
        await _client.SendFriendMessageAsync(_options.Destination,
            MessageChain.Construct(new Plain($"Message from Group {friend.Nickname}({friend.Identity}): ")));
        await _client.SendFriendMessageAsync(_options.Destination, chain);
    }
}