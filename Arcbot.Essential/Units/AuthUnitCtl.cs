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
    public class AuthUnitCtl : UnitBase
    {
        [Receive(MessageEventType.Group)]
        [Extract("!auth.grant.limit {who} {permission} {limit}")]
        [CheckTicket("whosyourdaddy")]
        public async Task AuthLimited(long who, Group group, string permission, int limit)
        {
            var member = new Member() { Identity = who, Group = new Lazy<Group>(group) };
            member.GrantLimited(permission, limit);
            await group.SendAsync($"[hyper.at({who})]拿到了 {permission} 许可. 该许可具有 {limit} 次使用机会.".MakeMessageChain());
        }

        [Receive(MessageEventType.Group)]
        [Extract("!auth.grant.expiry {who} {permission} {timestamp}")]
        [CheckTicket("whosyourdaddy")]
        public async Task AuthExpiry(long who, Group group, string permission, long timestamp)
        {
            var member = new Member() { Identity = who, Group = new Lazy<Group>(group) };
            var expire = new DateTime(1970, 1, 1).AddSeconds(timestamp);
            member.GrantExpiry(permission, expire);
            await group.SendAsync($"[hyper.at({who})]拿到了 {permission} 许可. 许可将在 {expire} 后过期.".MakeMessageChain());
        }

        [Receive(MessageEventType.Group)]
        [Extract("!auth.grant {who} {permission}")]
        [CheckTicket("whosyourdaddy")]
        public async Task AuthNormal(long who, Group group, string permission)
        {
            var member = new Member() { Identity = who, Group = new Lazy<Group>(group) };
            member.Grant(permission);
            await group.SendAsync($"[hyper.at({who})]拿到了{permission}许可.".MakeMessageChain());
        }

        [Receive(MessageEventType.Group)]
        [Extract("!auth.revoke {who} {permission}")]
        [CheckTicket("whosyourdaddy")]
        public async Task Revoke(long who, Group group, string permission)
        {
            var member = new Member() { Identity = who, Group = new Lazy<Group>(group) };
            member.RevokePermission(permission);
            await group.SendAsync($"不管之前有没有, 反正现在[hyper.at({who})]没有了{permission}许可.".MakeMessageChain());
        }

        [Receive(MessageEventType.Group)]
        [Extract("!auth.list {who}")]
        public async Task List(long who, Group group, IApiClient client)
        {
            var member = new Member() { Identity = who, Group = new Lazy<Group>(group) };
            member = await client.RequestAsync(member);
            var permissions = member.GetPermissions();
            await group.SendPlainAsync($"{member.DisplayName}({member.Identity}):\n{string.Join('\n', permissions)}");
        }
    }
}
