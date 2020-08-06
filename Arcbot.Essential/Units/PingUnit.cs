using Hyperai.Events;
using Hyperai.Relations;
using Hyperai.Services;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.Authorization.Attributes;
using HyperaiShell.Foundation.ModelExtensions;
using System;
using System.Threading.Tasks;

namespace Arcbot.Essential.Units
{
    public class PintUnit : UnitBase
    {
        [Receive(MessageEventType.Group)]
        [Extract("!ping")]
        public async Task Ping(Group group)
        {
            await group.SendPlainAsync("pong!");
        }
    }
}
