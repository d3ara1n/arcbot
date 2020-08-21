using System;
using System.Threading;
using Hyperai.Events;
using Hyperai.Relations;
using Hyperai.Units;
using Hyperai.Units.Attributes;
using HyperaiShell.Foundation.Authorization.Attributes;
using HyperaiShell.Foundation.ModelExtensions;

namespace Arcbot.GuessNumber.Units
{
    public class GuessNumberHackUnit: UnitBase
    {
        private readonly IUnitService _service;

        public GuessNumberHackUnit(IUnitService service)
        {
            _service = service;
        }

        // [Receive(MessageEventType.Group)]
        [Extract("!guess.hack [hyper.at({bot})]")]
        [CheckTicket("guess.hack")]
        public void Start(Group group, long bot)
        {
            string cmd = "/猜数字";
            group.SendPlainAsync(cmd);
            Thread.Sleep(1000);
            _service.WaitOne(Channel.Create(bot, group.Identity), NumberGuess, TimeSpan.FromSeconds(60));
            group.SendPlainAsync("/1234");
        }

        private void NumberGuess(MessageContext context)
        {
            context.ReplyAsync("/1234".MakeMessageChain()).Wait();
        }
    }
}