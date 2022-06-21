using System;
using System.Collections.Generic;
using System.Linq;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Abstractions.Relations;
using Onebot.Protocol;
using Onebot.Protocol.Models.Actions;
using Onebot.Protocol.Models.Events;
using Onebot.Protocol.Models.Events.Message;
using Onebot.Protocol.Models.Events.Meta;
using Onebot.Protocol.Models.Messages;
using Onebot.Protocol.Models.Receipts;

namespace Arcbot;

public static class ModelConversionExtensions
{
    public static GenericEventArgs ToHyperai(this EventBase evt, OnebotClient client) => evt switch
    {
        UnknownEvent it => new UnknownEventArgs() { Data = it.RawObject },
        HeartbeatEvent it => new UnknownEventArgs() { Data = it },
        GroupMessageEvent it => new GroupMessageEventArgs()
        {
            Group = client.GetHyperaiGroupAsync(long.Parse(it.GroupId)).Result,
            Message = it.Message.ToHyperai(),
            Sender = client.GetHyperaiMemberAsync(long.Parse(it.GroupId), long.Parse(it.UserId)).Result
        },

        _ => new UnknownEventArgs() { Data = evt },
    };

    public static MessageChain ToHyperai(this Message message) => new MessageChain(
        message.Select<MessageSegment, MessageElement>(x => x.Type switch
        {
            "text" => new Plain(x.Data["text"]),
            "mention" => new At(long.Parse(x.Data["user_id"])),
            "mention_all" => new AtAll(),
            "reply" => new Quote(x.Data["message_id"]),
            "image" => new Image(new Uri(x.Data.ContainsKey("url")
                ? x.Data["url"]
                : $"https://gchat.qpic.cn/gchatpic_new/0/{x.Data["file_id"]}/0?term=3")),
            _ => new Plain($"[Onebot] Unsupported message element:{x.Type}")
        }));

    public static Message ToOnebot(this MessageChain chain, OnebotClient client) => new Message(chain.Select(x =>
        x switch
        {
            At it => MessageSegment.Mention(it.Identity.ToString()),
            AtAll => MessageSegment.MentionAll(),
            Quote it => MessageSegment.Reply(it.MessageId, null),
            Image it => MessageSegment.Image(client.UploadFileAsync(it.Source.AbsoluteUri).Result.FileId),
            Plain it => MessageSegment.Text(it.Text),
            _ => MessageSegment.Text($"[HyperaiX] Unsupported message element: {x.TypeName}")
        }).ToList());
}