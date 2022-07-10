using System;
using System.Collections.Generic;
using System.Linq;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using Microsoft.Extensions.Caching.Memory;
using Onebot.Protocol;
using Onebot.Protocol.Models.Events;
using Onebot.Protocol.Models.Events.Message;
using Onebot.Protocol.Models.Events.Meta;
using Onebot.Protocol.Models.Messages;

namespace Arcbot;

public static class ModelConversionExtensions
{
    public static GenericEventArgs ToHyperai(this EventBase evt, OnebotClient client, IMemoryCache cache)
    {
        return evt switch
        {
            UnknownEvent it => new UnknownEventArgs { Data = it.RawObject },
            HeartbeatEvent it => new UnknownEventArgs { Data = it },
            GroupMessageEvent it => new GroupMessageEventArgs
            {
                Group = client.GetHyperaiGroupAsync(long.Parse(it.GroupId), cache).Result,
                Message = it.Message.ToHyperai(),
                Sender = client.GetHyperaiMemberAsync(long.Parse(it.GroupId), long.Parse(it.UserId), cache).Result
            },
            PrivateMessageEvent it => new FriendMessageEventArgs
            {
                Message = it.Message.ToHyperai(),
                Sender = client.GetHyperaiFriendAsync(long.Parse(it.UserId), cache).Result
            },
            _ => new UnknownEventArgs { Data = evt }
        };
    }

    public static MessageChain ToHyperai(this Message message)
    {
        return new MessageChain(message.Select<MessageSegment, MessageElement>(
            x => x.Type switch
            {
                "text" => new Plain(x.Data["text"].ToString()),
                "mention" => new At(long.Parse(x.Data["user_id"].ToString()!)),
                "mention_all" => new AtAll(),
                "reply" => new Quote(x.Data["message_id"].ToString()),
                "face" => new Face((int)(long)x.Data["id"]),
                "image" => x.Data.ContainsKey("flash") && x.Data["flash"].Equals(true)
                    ? new Flash(new Uri($"id://{x.Data["file_id"]}"))
                    : new Image(new Uri($"id://{x.Data["file_id"]}")),
                "voice" => new Audio(new Uri($"id://{x.Data["file_id"]}")),
                _ => new Plain($"[Onebot] Unsupported message element:{x.Type}")
            }));
    }

    public static Message ToOnebot(this MessageChain chain, OnebotClient client)
    {
        return new Message(chain.Select(x =>
            x switch
            {
                At it => MessageSegment.Mention(it.Identity.ToString()),
                AtAll => MessageSegment.MentionAll(),
                Quote it => MessageSegment.Reply(it.MessageId, null),
                Image it => MessageSegment.Image(it.Source.Scheme switch
                {
                    "id" => it.Source.Host,
                    _ => client.UploadFileAsync(it.Source.AbsoluteUri).Result.FileId
                }),
                Flash it => MessageSegment.From("image", new[]
                {
                    new KeyValuePair<string, object>("file_id", it.Source.Scheme switch
                    {
                        "id" => it.Source.Host,
                        _ => client.UploadFileAsync(it.Source.AbsolutePath).Result.FileId
                    }),
                    new KeyValuePair<string, object>("flash", true)
                }),
                Face it => MessageSegment.From("face", new[]
                {
                    new KeyValuePair<string, object>("id", it.FaceId)
                }),
                Plain it => MessageSegment.Text(it.Text),
                Audio it => MessageSegment.Voice(it.Source.Host),
                _ => MessageSegment.Text($"[HyperaiX] Unsupported message element: {x.TypeName}")
            }).ToList());
    }
}