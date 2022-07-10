using System;
using Arcbot.Data;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units;
using HyperaiX.Units.Attributes;
using Microsoft.Extensions.Logging;

namespace Arcbot.Units;

public class MessageRecordUnit : UnitBase
{
    private readonly ArcContext _context;
    private readonly ILogger _logger;

    public MessageRecordUnit(ILogger<MessageRecordUnit> logger, ArcContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Receiver(MessageEventType.Group)]
    public void GroupRecord(MessageChain chain, Group group, Member member)
    {
        var record = new MessageModel
        {
            Sender = member.Identity,
            Group = group.Identity,
            Content = chain.ToString(),
            Time = DateTime.Now
        };

        _context.Messages.Add(record);
    }

    [Receiver(MessageEventType.Friend)]
    public void FriendRecord(MessageChain chain, Friend friend)
    {
        var record = new MessageModel
        {
            Sender = friend.Identity,
            Group = 0,
            Content = chain.ToString(),
            Time = DateTime.Now
        };

        _context.Messages.Add(record);
    }
}