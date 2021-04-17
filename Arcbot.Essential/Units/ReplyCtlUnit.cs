using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Arcbot.Essential.Services;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.Authorization.Attributes;
using HyperaiShell.Foundation.ModelExtensions;

namespace Arcbot.Essential.Units
{
    public class ReplyCtlUnit : UnitBase
    {
        private readonly ReplyService _service;

        public ReplyCtlUnit(ReplyService service)
        {
            _service = service;
        }

        [Receive(MessageEventType.Group)]
        [Description("在群里添加一条消息自动回复规则")]
        [CheckTicket("reply.control.register")]
        [Extract("!reply.add {trigger} {reply}")]
        public async Task Register(Group group, Member member, MessageChain trigger, MessageChain reply)
        {
            _service.Add(group.Identity, member.Identity, trigger, reply);
            var builder = new MessageChainBuilder();
            builder.AddPlain("当:\n");
            foreach (var msg in trigger)
            {
                builder.Add(msg);
            }

            builder.AddPlain("\n则:\n");
            foreach (var msg in reply)
            {
                builder.Add(msg);
            }

            await group.SendAsync(builder.Build());
        }

        [Receive(MessageEventType.Group)]
        [Description("移除一条规则")]
        [Extract("!reply.remove {id}")]
        [CheckTicket("reply.control")]
        public async Task Unregister(Group group, int id)
        {
            _service.Remove(id);
            await group.SendPlainAsync("移除了，也许没有，总之它不存在了");
        }

        [Receive(MessageEventType.Group)]
        [Description("查询本群的规则")]
        [Extract("!reply.list")]
        [CheckTicket("reply.control")]
        public async Task List(Group group)
        {
            var list = _service.List(group.Identity);
            StringBuilder builder = new($"这里, {group.Name}({group.Identity})\n");
            foreach (var piece in list)
            {
                builder.AppendLine($"[{piece.Id}]");
                builder.AppendLine($"    {piece.Trigger}");
                builder.AppendLine($"  --{piece.Reply}");
            }

            await group.SendPlainAsync(builder.ToString());
        }
    }
}