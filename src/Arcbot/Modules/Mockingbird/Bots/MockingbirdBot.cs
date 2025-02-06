using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Bots;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Extensions.QQ.Roles;

namespace Arcbot.Modules.Mockingbird.Bots;

public class MockingbirdBot(IEndClient client) : BotBase
{
    public override async Task OnEventAsync(GenericEventArgs args)
    {
        if (args is MessageEventArgs { Chat: Group group, Sender: Member member, Message.Body: RichContent } evt)
        {
            if (Random.Shared.Next(512) == 0)
            {
                await client.WriteAsync(new SendMessageActionArgs(group, evt.Message));
            }
        }
    }
}