using HyperaiX.Abstractions.Bots;
using HyperaiX.Abstractions.Events;

namespace Arcbot.Modules.Fun.Bots;

public class FunBot: BotBase
{
    public override Task OnEventAsync(GenericEventArgs args)
    {
        return Task.CompletedTask;
    }
}
