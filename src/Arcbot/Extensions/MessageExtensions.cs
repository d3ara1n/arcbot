using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using Onebot.Protocol.Models.Messages;

namespace Arcbot.Extensions
{
    public static class MessageExtensions
    {
        public static MessageChain ToMessageChain(this IEnumerable<MessageCell> message) =>
            new(message.Any(x => x.Type == "forward")
                ? new MessageChain(new[] { new Unknown(message) }) // TODO: go fuck your mother
                : message.Select<MessageCell, MessageElement>(x => x.Type switch
                {
                    "text" => new Plain(x.Data["text"]),
                    "face" => new Face(Convert.ToInt32(x.Data["id"])),
                    "image" => x.Data.ContainsKey("type")
                        ? new Flash(new Uri(x.Data["url"]))
                        : new Image(new Uri(x.Data["url"])),
                    "record" => new Audio(new Uri(x.Data["url"])),
                    "video" => new Video(new Uri(x.Data["url"])),
                    "at" => x.Data["qq"] switch
                    {
                        "all" => new AtAll(),
                        _ => new At(Convert.ToInt64(x.Data["qq"]))
                    },
                    "reply" => new Quote(Convert.ToInt64(x.Data["id"])),
                    _ => new Unknown(x)
                }));

        public static IEnumerable<MessageCell> ToMessage(this MessageChain chain) => chain.Select(x => x switch
        {
            Plain plain => new MessageCell()
                { Type = "text", Data = new Dictionary<string, string>() { { "text", plain.Text } } },
            Image image => new MessageCell()
            {
                Type = "image",
                Data = new Dictionary<string, string>()
                    { { "url", image.Source.AbsoluteUri }, { "file", image.Source.AbsoluteUri } }
            },
            Flash flash => new MessageCell()
            {
                Type = "image",
                Data = new Dictionary<string, string>()
                    { { "url", flash.Source.AbsoluteUri }, { "file", flash.Source.AbsoluteUri }, { "type", "flash" } }
            },
            //TODO: complete all
            _ => new MessageCell()
                { Type = "unknown", Data = new Dictionary<string, string>() }
        });
    }
}