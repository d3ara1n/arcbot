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
    public class ReplyCtlUnit: UnitBase
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
            await group.SendPlainAsync("当:");
            await group.SendAsync(trigger);
            await group.SendPlainAsync("则:");
            await group.SendAsync(reply);
        }
        
        [Receive(MessageEventType.Group)]
        [Description("移除一条规则")]
        [Extract("!reply.remove {id}")]
        [CheckTicket("reply.control")]
        public async Task Unregister(Group group, string id)
        {
            if (Guid.TryParse(id, out Guid guid))
            {
                _service.Remove(guid);
                await group.SendPlainAsync("移除了，也许没有，总之它不存在了");
            }
            else
            {
                await group.SendPlainAsync("id格式不对");
            }
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