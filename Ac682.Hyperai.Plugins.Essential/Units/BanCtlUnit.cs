using System.Threading.Tasks;
using System.Linq;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.Authorization.Attributes;
using Hyperai.Messages.ConcreteModels;
using HyperaiShell.Foundation.Services;
using Hyperai.Relations;
using HyperaiShell.Foundation.ModelExtensions;

namespace Ac682.Hyperai.Plugins.Essential
{
    public class BanCtlUnit : UnitBase
    {
        private readonly IBlockService _service;

        public BanCtlUnit(IBlockService service)
        {
            _service = service;
        }

        [Receive(MessageEventType.Group)]
        [Extract("!ban {who} {reason}")]
        [CheckTicket("blacklist.control.add")]
        public async Task Ban(Group group, MessageChain who, string reason)
        {
            var at = (At)who.FirstOrDefault(x => x is At);
            if (at != null)
            {
                _service.Ban(at.TargetId, reason);
                await group.SendPlainAsync($"{at.TargetId} has been banned: {reason}.");
            }
        }

        [Receive(MessageEventType.Friend)]
        [Extract("!ban {who} {reason}")]
        [CheckTicket("blacklist.control.add")]
        public async Task Ban(Friend friend, long who, string reason)
        {
            _service.Ban(who, reason);
            await friend.SendPlainAsync($"{who} has been banned: {reason}.");
        }

        [Receive(MessageEventType.Group)]
        [Extract("!deban {who}")]
        [CheckTicket("blacklist.control.remove")]
        public async Task Deban(Group group, MessageChain who)
        {
            var at = (At)who.FirstOrDefault(x => x is At);
            if (at != null)
            {
                _service.Deban(at.TargetId);
                await group.SendPlainAsync($"{at.TargetId} has been debanned.");
            }
        }

        [Receive(MessageEventType.Friend)]
        [Extract("!deban {who}")]
        [CheckTicket("blacklist.control.remove")]
        public async Task Deban(Friend friend, long who)
        {
            _service.Deban(who);
            await friend.SendPlainAsync($"{who} has been debanned.");
        }

        [Receive(MessageEventType.Friend)]
        [Extract("!isbanned {who}")]
        [CheckTicket("blacklist.control.query")]
        public async Task Check(Friend friend, long who)
        {
            _ = _service.IsBanned(who, out string reason);
            await friend.SendPlainAsync($"{who} has been banned for {reason}.");
        }

        [Receive(MessageEventType.Group)]
        [Extract("!isbanned {who}")]
        [CheckTicket("blacklist.control.query")]
        public async Task Check(Group group, MessageChain who)
        {
            var at = (At)who.FirstOrDefault(x => x is At);
            if (at != null)
            {
                _ = _service.IsBanned(at.TargetId, out string reason);
                await group.SendPlainAsync($"{at.TargetId} has been banned for {reason}.");
            }
        }
    }
}