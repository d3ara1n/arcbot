using System;
using System.ComponentModel;
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
    public class ReminderUnit: UnitBase
    {
        
        [Receive(MessageEventType.Friend)]
        [Description("在指定日期发送预设好的消息")]
        [Extract("!reminder {dateTime} {message}")]
        public async Task RemindMe(Friend friend, DateTime dateTime, MessageChain message)
        {
            BackgroundJob.Schedule( () => friend.SendAsync(message), dateTime);
            await friend.SendPlainAsync($"任务时间被设定在了{dateTime}");
        }

        [Receive(MessageEventType.Group)]
        [Description("在指定日期发送预设好的消息")]
        [Extract("!reminder {dateTime} {message}")]
        [RequiredTicket("reminder.schedule")]
        public async Task RemindMe(Group group, Member member, DateTime dateTime, MessageChain message)
        {
            BackgroundJob.Schedule(() => group.SendAsync(message), dateTime);
            await group.SendPlainAsync($"任务时间被设定在了{dateTime}");
        }
    }
}