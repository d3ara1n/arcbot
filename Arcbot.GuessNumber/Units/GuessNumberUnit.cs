using System;
using System.Linq;
using System.Threading.Tasks;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.ModelExtensions;

namespace Arcbot.GuessNumber.Units
{
    public class GuessNumberUnit : UnitBase
    {
        private readonly GuessBox box = new GuessBox();
        private int trail = 0;

        [Receive(MessageEventType.Group)]
        [Extract("!guess.fuck")]
        public async Task Start(Member member, Group group, MessageChain raw)
        {
            box.Start();
            var builder = raw.CanBeReplied() ? raw.MakeReply() : new MessageChainBuilder();
            builder.AddPlain("4位数字已生成， 输入四位数字 xxxx 来猜测");
            var task = group.SendAsync(builder.Build());
            member.Await(NumberGuess, 60 * 1000);
            await task;
        }

        private void NumberGuess(MessageContext context)
        {
            var msg = context.Message.AsReadable().ToString();
            if(msg == "!quit")
            {
                context.ReplyAsync("wdnmd，这太难了，我不玩了！".MakeMessageChain()).Wait();
                return;
            }
            try
            {
                int[] array = msg.Select(x => int.Parse(x.ToString())).ToArray();
                trail++;
                (int a, int b) = box.Calculate(array);
                if (a == 4)
                {
                    context.ReplyAsync($"[hyper.at({context.User.Identity})]笨笨， 您太牛了，{trail}步之内出答案.".MakeMessageChain()).Wait();
                }
                else
                {
                    context.ReplyAsync($"[hyper.at({context.User.Identity})]{a}A{b}B".MakeMessageChain()).Wait();
                    ((Member)context.User).Await(NumberGuess, 60 * 1000);
                }
            }
            catch
            {
                context.ReplyAsync($"[hyper.at({context.User.Identity})]输入格式有误.".MakeMessageChain()).Wait();
            }

        }
    }
}