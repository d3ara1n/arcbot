using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Arcbot.Essential.Models.ProfileInventory;
using Arcbot.Essential.Services;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.ModelExtensions;

namespace Arcbot.Essential.Units
{
    public class DailyUnit : UnitBase
    {
        private readonly ProfileService _service;

        public DailyUnit(ProfileService service)
        {
            _service = service;
        }

        [Receive(MessageEventType.Group)]
        [Extract("签到")]
        [Description("签到领硬币")]
        public async Task Signin(Member member, Group group, MessageChain raw)
        {
            var coin = _service.Inspect<Coin>(member);
            var now = DateTime.Now;
            if (coin == null || (coin.LastModified.Date < now.Date))
            {
                int up = coin != null && coin.LastModified.Date.AddDays(1) == now.Date ? 15 : 10;
                _service.PutCoin(member, up);
                var builder = raw.CanBeReplied() ? raw.MakeReply() : new MessageChainBuilder();
                builder.AddPlain($"签到成功, 硬币+{up}🎉");
                await group.SendAsync(builder.Build());
            }
            else
            {
                var builder = raw.CanBeReplied() ? raw.MakeReply() : new MessageChainBuilder();
                builder.AddPlain("你已经签到过了😥");
                await group.SendAsync(builder.Build());
            }
        }

        [Receive(MessageEventType.Group)]
        [Extract("!profile")]
        [Description("查看我的信息")]
        public async Task Profile(Member member, Group group, MessageChain raw)
        {
            var builder = raw.CanBeReplied() ? raw.MakeReply() : new MessageChainBuilder();
            StringBuilder sb = new StringBuilder();
            sb.Append("你, ");
            sb.AppendLine(member.DisplayName);
            sb.AppendLine("\n库存: ");
            int cnt = 0;
            foreach (var item in _service.Items(member))
            {
                cnt++;
                sb.AppendLine($"{cnt}.{item.DisplayName} - {item.Stack} 个");
            }
            builder.AddPlain(sb.ToString().Trim());
            await group.SendAsync(builder.Build());
        }
    }
}