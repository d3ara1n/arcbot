using System.ComponentModel;
using System.Threading.Tasks;
using Hyperai.Events;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.ModelExtensions;

namespace Arcbot.Essential.Units
{
    public class PintUnit : UnitBase
    {
        [Receive(MessageEventType.Group)]
        [Extract("!ping")]
        [Description("pong!")]
        public async Task Ping(Group group)
        {
            await group.SendPlainAsync("pong!");
        }
    }
}