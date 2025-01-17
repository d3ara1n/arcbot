using HyperaiX.Abstractions.Bots;
using HyperaiX.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace Arcbot.Modules.Debugging.Bots;

public class DebugBot(ILogger<DebugBot> logger) : BotBase
{
    public override Task OnEventAsync(GenericEventArgs args)
    {
        return Task.CompletedTask;
    }
}