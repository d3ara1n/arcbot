using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Abstractions.Messages.Payloads.Elements;
using HyperaiX.Abstractions.Units;
using HyperaiX.Extensions.QQ.Roles;

namespace Arcbot.Modules.Debugging.Units;

public class DebugUnit : UnitBase
{
    [Receive<Group>]
    public MessageEntity? Ping()
    {
        return Context.Message.Body is RichContent { Elements: [Text("!ping")] }
            ? MessageEntity.CreateText("pong!")
            : null;
    }
}