using Ac682.Hyperai.Plugins.Essential.Services;
using Hyperai.Events;
using Hyperai.Messages;
using Hyperai.Relations;
using HyperaiShell.Foundation.Bots;
using HyperaiShell.Foundation.ModelExtensions;

namespace Ac682.Hyperai.Plugins.Essential.Bots
{
    public class EchoBot : BotBase
    {
        private readonly EchoService _service;

        public EchoBot(EchoService service)
        {
            _service = service;
        }

        public override void OnFriendMessage(object sender, FriendMessageEventArgs args)
        {
            if (_service.IsOn(args.User.Identity)) args.User.SendAsync(args.Message).Wait();
        }

        public override void OnGroupMessage(object sender, GroupMessageEventArgs args)
        {
            if (_service.IsOn(args.Group.Identity)) args.Group.SendAsync(args.Message).Wait();
        }
    }
}