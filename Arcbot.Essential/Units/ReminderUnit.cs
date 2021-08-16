using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.Authorization.Attributes;
using HyperaiShell.Foundation.ModelExtensions;

namespace Arcbot.Essential.Units
{
    public class ReminderUnit : UnitBase
    {
        private readonly IBackgroundJobClient _jobClient;
        public ReminderUnit(IBackgroundJobClient jobClient)
        {
            _jobClient = jobClient;
        }

        [Receive(MessageEventType.Friend)]
        [Description("在指定日期发送预设好的消息")]
        [Extract("!reminder {dateTime} {message}")]
        public async Task RemindMe(Friend friend, DateTime dateTime, MessageChain message)
        {
            _jobClient.Schedule(() => SendToFriend(friend.Identity, message), dateTime);
            await friend.SendPlainAsync($"任务时间被设定在了{dateTime},距离现在还有{CalcDateTime(dateTime)}");
        }

        [Receive(MessageEventType.Group)]
        [Description("在指定日期发送预设好的消息")]
        [Extract("!reminder {dateTime} {message}")]
        [RequiredTicket("reminder.schedule")]
        public async Task RemindMe(Group group, Member member, DateTime dateTime, MessageChain message)
        {
            _jobClient.Schedule(() => SendToGroup(group.Identity, message), dateTime);
            await group.SendPlainAsync($"任务时间被设定在了{dateTime},距离现在还有{CalcDateTime(dateTime)}");
        }

        private string CalcDateTime(DateTime dateTime)
        {
            var delta = dateTime - DateTime.Now;
            StringBuilder sb = new();
            if (delta.Days > 0)
            {
                sb.Append($"{delta.Days}天");
            }

            sb.Append($"{delta.Hours}小时{delta.Minutes}分{delta.Seconds}秒");

            return sb.ToString();
        }

        public void SendToFriend(long id, MessageChain chain)
        {
            var friend = new Friend() { Identity = id };
            friend.SendAsync(chain).Wait();
        }

        public void SendToGroup(long id, MessageChain chain)
        {
            var group = new Group() { Identity = id };
            group.SendAsync(chain).Wait();
        }
    }
}