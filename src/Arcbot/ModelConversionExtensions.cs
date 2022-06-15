using System;
using System.Collections.Generic;
using System.Linq;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Relations;
using Onebot.Protocol.Models.Events;
using Onebot.Protocol.Models.Events.Message;
using Onebot.Protocol.Models.Events.Meta;
using Onebot.Protocol.Models.Messages;

namespace Arcbot;

public static class ModelConversionExtensions
{
    public static GenericEventArgs ToHyperai(this EventBase evt) => evt switch
    {
        UnknownEvent it => new UnknownEventArgs() { Data = it.RawObject },
        HeartbeatEvent it => new UnknownEventArgs() { Data = it },
        GroupMessageEvent it => new GroupMessageEventArgs()
        {
            Group = new Group()
            {
                Identity = long.Parse(it.GroupId),
                Members = new Lazy<IEnumerable<Member>>(),
                Name = "Unknown",
                Owner = new Lazy<Member>()
            },
            Message = it.Message.ToHyperai(),
            Sender = new Member()
            {
                Identity = long.Parse(it.UserId),
                DisplayName = "Unknown"
            }
        },

        _ => new UnknownEventArgs() { Data = evt },
    };

    public static MessageChain ToHyperai(this Message message) => new MessageChain(message.Select<MessageSegment, MessageElement>(x => x.Type switch
    {
        "text" => new Plain(x.Data["text"]),
        "mention" => new At(long.Parse(x.Data["user_id"])),
        "mention_all" => new AtAll(),
        "reply" => new Quote(x.Data["message_id"]),
        _ => new Plain($"Unsupported message element:{x.Type}")
    }));
}