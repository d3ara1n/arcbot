using System.Threading.Tasks;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units;
using HyperaiX.Units.Patterns;
using HyperaiX.Units.Patterns.Commands;

namespace Arcbot.Units
{
    public class PingUnit : UnitBase
    {
        public PingUnit()
        {
            Builder.Command(nameof(PingAsync), configure => configure
                .Group()
                .WithText("ping"));
        }

        public Task PingAsync(Group group)
        {
            await group.SendAsync(MessageChain.Construct(new Plain("pong!")));
        }
    }
}