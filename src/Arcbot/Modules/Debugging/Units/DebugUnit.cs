using HyperaiX.Abstractions.Units;
using HyperaiX.Extensions.QQ.Roles;

namespace Arcbot.Modules.Debugging.Units;

public class DebugUnit : UnitBase
{
    [Receive<Conversation>]
    public async Task Echo()
    {
        await Context.SendAsync(Context.Message);
    }
}