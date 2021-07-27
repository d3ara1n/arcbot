using System.ComponentModel;
using System.Threading.Tasks;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.ModelExtensions;

namespace Arcbot.Essential.Units
{
    public class EchoUnit: UnitBase
    {
        [Receive(MessageEventType.Group)]
        [Extract("!echo {message}")]
        [Description("来什么出什么")]
        public async Task Echo(Group group, MessageChain message)
        {
            await group.SendAsync(message);
        }
        
        [Receive(MessageEventType.Friend)]
        [Extract("!echo {message}")]
        [Description("来什么出什么")]
        public async Task Echo(Friend friend, MessageChain message)
        {
            await friend.SendAsync(message);
        }
    }
}