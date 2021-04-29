using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Services;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.Authorization.Attributes;
using HyperaiShell.Foundation.ModelExtensions;

namespace Arcbot.Essential.Units
{
    public class DebugUnit : UnitBase
    {
        [Receive(MessageEventType.Group)]
        [Extract("!about.self")]
        [Description("关于我自己")]
        public async Task AboutSelf(Group group, Self self)
        {
            StringBuilder builder = new();
            builder.AppendLine($"{self.Nickname}({self.Identity})");
            builder.AppendLine($"Friends: {self.Friends.Value.Count()}");
            builder.AppendLine($"Groups: {self.Groups.Value.Count()}");

            await group.SendPlainAsync(builder.ToString());
        }

        [Receive(MessageEventType.Group)]
        [Extract("!about.me")]
        [Description("看看自己")]
        public async Task AboutMe(Group group, Member member)
        {
            StringBuilder builder = new();
            builder.AppendLine($"{member.Nickname}({member.Identity})");
            builder.AppendLine($"DisplayName: {member.DisplayName}");
            builder.AppendLine($"Role: {member.Role}");
            builder.AppendLine($"Title: {member.Title}");

            await group.SendPlainAsync(builder.ToString());
        }

        [Receive(MessageEventType.Group)]
        [Extract("!about.group")]
        [Description("看看群")]
        public async Task AboutGroup(Group group)
        {
            StringBuilder builder = new();
            builder.AppendLine($"{group.Name}({group.Identity})");
            builder.AppendLine($"Owner: {group.Owner.Value.DisplayName ?? "(UNDEFINED)"}");
            builder.AppendLine($"Members: {group.Members.Value?.Count()}");

            await group.SendPlainAsync(builder.ToString());
        }

        [Receive(MessageEventType.Friend)]
        [Extract("!say.friend {id} {message}")]
        [Description("发送给好友一条消息")]
        [CheckTicket("whosyourdaddy")]
        public async Task SayF(long id, MessageChain message)
        {
            var friend = new Friend() {Identity = id};
            await friend.SendAsync(message);
        }

        [Receive(MessageEventType.Friend)]
        [Extract("!say.group {id} {message}")]
        [Description("发送给群一条消息")]
        [CheckTicket("whosyourdaddy")]
        public async Task SayG(long id, MessageChain message)
        {
            var group = new Group() {Identity = id};
            await group.SendAsync(message);
        }
    }
}