using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Abstractions.Messages.Payloads.Elements;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Extensions.QQ.Roles;
using Microsoft.Extensions.Logging;

namespace Arcbot.Clients;

public class DummyClient(ILogger<DummyClient> logger) : IEndClient
{
    private static readonly Friend MISIDE = new(114514, "米塔", null);

    private readonly GenericEventArgs[] _events =
    [
        new MessageEventArgs(new Conversation(MISIDE), MISIDE, MISIDE,
            new MessageEntity("没有预览", new RichContent([new Text("!ping")]), new Dictionary<string, object>(),
                DateTimeOffset.UtcNow))
    ];

    private int _cursor;

    public Task ConnectAsync(CancellationToken token)
    {
        return Task.CompletedTask;
    }

    public Task DisconnectAsync(CancellationToken token)
    {
        return Task.CompletedTask;
    }

    public ValueTask<GenericEventArgs> ReadAsync(CancellationToken token)
    {
        if (_cursor < _events.Length)
        {
            var evt = _events[_cursor++];
            logger.LogInformation("Inbound {}", evt);
            return ValueTask.FromResult(evt);
        }

        while (true)
        {
            token.ThrowIfCancellationRequested();
            Thread.Sleep(1000);
        }
    }

    public ValueTask<GenericReceiptArgs> WriteAsync(GenericActionArgs action, CancellationToken token)
    {
        logger.LogInformation("Outbound {}", action);
        return ValueTask.FromResult(new GenericReceiptArgs());
    }
}