using Arcbot.Essential.Services;
using Hyperai.Events;
using Hyperai.Messages;
using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.Data;
using HyperaiShell.Foundation.ModelExtensions;
using HyperaiShell.Foundation.Plugins;

namespace Arcbot.Essential.Bots
{
    public class ReplyBot: BotBase
    {
        private readonly ReplyService _service;

        public ReplyBot(ReplyService service)
        {
            _service = service;
        }

        public override void OnGroupMessage(object sender, GroupMessageEventArgs args)
        {
            var reply = _service.Get(args.Message.AsReadable(), args.Group.Identity);
            if (reply != null)
            {
                args.Group.SendAsync(reply.Reply.MakeMessageChain()).Wait();
            }
        }
    }
}