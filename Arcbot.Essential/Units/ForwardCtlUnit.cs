using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Arcbot.Essential.Models;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.Authorization.Attributes;
using HyperaiShell.Foundation.Data;
using HyperaiShell.Foundation.ModelExtensions;
using HyperaiShell.Foundation.Plugins;

namespace Arcbot.Essential.Units
{
    public class ForwardCtlUnit: UnitBase
    {
        private readonly IRepository _repository;
        
        public ForwardCtlUnit(IPluginRepository<PluginEntry> repository)
        {
            _repository = repository.Value;
        }


        [Receive(MessageEventType.Group)]
        [Extract("!forward.to.group {dest}")]
        [Description("转发消息到群")]
        [RequiredTicket("forward.control")]
        public async Task TrackGroup(Group group, long dest)
        {
            _repository.Store(new ForwardChannel(RelationMatcher.FromGroup(group.Identity),dest, MessageEventType.Group));
            await group.SendPlainAsync($"转发本群消息到群 {dest}");
        }

        [Receive(MessageEventType.Group)]
        [Extract("!forward.to.me")]
        [Description("转发群消息给自己")]
        [RequiredTicket("forward.control")]
        public async Task TrackGroup(Group group, Member sender)
        {
            _repository.Store(new ForwardChannel(RelationMatcher.FromGroup(group.Identity), sender.Identity, MessageEventType.Friend));
            await group.SendPlainAsync($"转发本群消息到好友 {sender.Nickname}");
        }

        [Receive(MessageEventType.Group)]
        [Extract("!forward.list")]
        [Description("枚举全部规则")]
        [RequiredTicket("forward.control")]
        public async Task List(Group group)
        {
            var list = _repository.Query<ForwardChannel>().ToList();
            StringBuilder builder = new();
            foreach (var ele in list)
            {
                builder.AppendLine(
                    $"[{ele.Id}]{ele.Rule.Expression} to {(ele.DestinationType switch {MessageEventType.Friend => "f", MessageEventType.Group => "g", _ => "s"})}{ele.Destination}");
            }
            await group.SendPlainAsync(builder.ToString().TrimEnd());
        }

        [Receive(MessageEventType.Group)]
        [Extract("!forward.remove {id}")]
        [Description("移除一条规则")]
        [RequiredTicket("forward.control")]
        public async Task Remove(Group group, int id)
        {
            _repository.Delete<ForwardChannel>(id);
            await group.SendPlainAsync("移除了，也许没有，总之它不存在了");
        }
    }
}