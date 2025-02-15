using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.Builders;
using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Abstractions.Messages.Payloads.Elements;
using HyperaiX.Abstractions.Units;
using HyperaiX.Abstractions.Units.Filters;
using HyperaiX.Extensions.QQ.Messages.Builders;
using HyperaiX.Extensions.QQ.Messages.Payloads.Elements;
using HyperaiX.Extensions.QQ.Roles;

namespace Arcbot.Modules.Debugging.Units;

public class DebugUnit : UnitBase
{
    [Receive<Group>]
    [Extract("!ping")]
    public MessageEntity? Ping()
    {
        return Context.Message.Body is RichContent { Elements: [Text("!ping")] }
            ? MessageEntity.CreateText("pong!")
            : null;
    }

    [Receive<Group>]
    [Extract("!ping {at:At} ")]
    public MessageEntity Ping(At at)
    {
        return Context.Self is Member self && self.Id == at.MemberId && Context.Sender is Member sender
            ? MessageEntity.Builder().RichContent().At(sender.Id, sender.DisplayName).Text(" pong!").Build()
            : MessageEntity.CreateText("pong!");
    }

    [Receive<Group>]
    public MessageEntity? Echo()
    {
        const string start = "!echo ";
        if (Context.Message.Body is RichContent { Elements: [Text text, ..] } content && text.Plain.StartsWith(start))
        {
            return Context.Message with
            {
                Body = content with
                {
                    Elements = content.Elements.Skip(1).Prepend(new Text(text.Plain[start.Length..])).ToList()
                }
            };
        }

        return null;
    }
}