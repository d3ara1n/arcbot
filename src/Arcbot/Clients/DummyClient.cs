using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Abstractions.Messages.Payloads.Elements;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Extensions.QQ.Roles;

namespace Arcbot.Clients;

public class DummyClient : IEndClient
{
    private static readonly Friend MISIDE = new Friend(114514, "米塔");

    private GenericEventArgs[] _events =
    [
        new MessageEventArgs(new Conversation(MISIDE), MISIDE,
            new MessageEntity(new RichContent([new Text("这是第一条消息")]), new Dictionary<string, object>(),
                DateTimeOffset.UtcNow))
    ];

    private int _cursor = 0;

    public Task ConnectAsync(CancellationToken token)
    {
        return Task.CompletedTask;
    }

    public Task DisconnectAsync(CancellationToken token)
    {
        return Task.CompletedTask;
    }

    public GenericEventArgs Read(CancellationToken token)
    {
        if (_cursor < _events.Length)
        {
            return _events[_cursor++];
        }

        while (true)
        {
            token.ThrowIfCancellationRequested();
            Thread.Sleep(1000);
        }
        
    }

    public GenericReceiptArgs Write(GenericActionArgs action, CancellationToken token)
    {
        return new GenericReceiptArgs();
    }
}